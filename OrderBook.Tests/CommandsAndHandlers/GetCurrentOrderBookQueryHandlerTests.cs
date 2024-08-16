using CSharpFunctionalExtensions;
using Moq;
using OrderBook.Application.Dto;
using OrderBook.Application.Handlers.OrderBook;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Services.Abstractions;


namespace OrderBook.Tests.CommandsAndHandlers
{
    public class GetCurrentOrderBookQueryHandlerTests
    {
        private const string ApiError = "API error";
        private readonly Mock<IBinanceService> _mockBinanceService;
        private readonly Mock<ISnapshotService> _mockSnapshotService;
        private readonly GetCurrentOrderBookQuery.Handler _handler;
        private static readonly string[] _mockBids = ["1", "2"];
        private static readonly string[] _mockAsks = ["3", "4"];

        public GetCurrentOrderBookQueryHandlerTests()
        {
            _mockBinanceService = new Mock<IBinanceService>();
            _mockSnapshotService = new Mock<ISnapshotService>();
            _handler = new GetCurrentOrderBookQuery.Handler(_mockBinanceService.Object, _mockSnapshotService.Object);
        }

        [Fact]
        public async Task Handle_OrderBookApiCallFails_ReturnFailure()
        {
            // Arrange
            _mockBinanceService.Setup(s => s.GetOrderBookAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<BinanceOrderBook>(ApiError));

            // Act
            var result = await _handler.Handle(new GetCurrentOrderBookQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("API error", result.Error);
            _mockSnapshotService.Verify(s => s.CreateSnapshotAsync(It.IsAny<BinanceOrderBook>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_AveragePriceApiCallFails_ReturnFailure()
        {
            // Arrange
            var mockOrderBook = new BinanceOrderBook
            { 
                Asks = new List<string[]>
                {
                    _mockAsks
                },
                Bids = new List<string[]> 
                {
                    _mockBids
                } 
            };

            _mockBinanceService.Setup(s => s.GetOrderBookAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(mockOrderBook));

            _mockBinanceService.Setup(s => s.GetAverageMarketPriceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<AverageMarketPrice>("Price API error"));

            // Act
            var result = await _handler.Handle(new GetCurrentOrderBookQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Price API error", result.Error);
            _mockSnapshotService.Verify(s => s.CreateSnapshotAsync(It.IsAny<BinanceOrderBook>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ApiCallsAreSuccessful_ReturnsSuccess()
        {
            // Arrange
            var mockOrderBook = new BinanceOrderBook
            {
                Asks = new List<string[]>
                {
                    _mockAsks
                },
                Bids = new List<string[]>
                {
                    _mockBids
                }
            };
            var mockAveragePrice = new AverageMarketPrice { Price = "20000" };

            _mockBinanceService.Setup(s => s.GetOrderBookAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(mockOrderBook));

            _mockBinanceService.Setup(s => s.GetAverageMarketPriceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(mockAveragePrice));

            _mockSnapshotService.Setup(s => s.CreateSnapshotAsync(It.IsAny<BinanceOrderBook>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(new GetCurrentOrderBookQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var expectedAsks = mockOrderBook.Asks
                .Select(x => new AskBidDto(Convert.ToDecimal(x[1]), Convert.ToDecimal(x[0])))
                .OrderByDescending(x => x.Price)
                .ToList();

            var expectedBids = mockOrderBook.Bids
                .Select(x => new AskBidDto(Convert.ToDecimal(x[1]), Convert.ToDecimal(x[0])))
                .OrderByDescending(x => x.Price)
                .ToList();

            Assert.Equal(expectedAsks, result.Value.Asks);
            Assert.Equal(expectedBids, result.Value.Bids);
            Assert.Equal(Convert.ToDecimal(mockAveragePrice.Price), result.Value.AveragePrice);

            _mockSnapshotService.Verify(s => s.CreateSnapshotAsync(mockOrderBook, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
