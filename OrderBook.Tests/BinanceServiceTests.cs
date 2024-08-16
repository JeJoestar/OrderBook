using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Options;
using OrderBook.Infrastructure.Services;
using System.Net;

namespace OrderBook.Tests.Infrastructure.Services
{
    public class BinanceServiceTests
    {
        private const string BinanceApiError = "Failed to extract the order book from Binance.";

        private readonly Mock<IBinanceApiOptions> _mockOptions;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly BinanceService _binanceService;

        public BinanceServiceTests()
        {
            _mockOptions = new Mock<IBinanceApiOptions>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _mockOptions.SetupGet(x => x.BinanceApiBaseUrl).Returns("https://api.binance.com/api/v3/");

            _binanceService = new BinanceService(_mockOptions.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetAverageMarketPriceAsync_ApiCallIsSuccessful_ReturnSuccess()
        {
            // Arrange
            var averageMarketPrice = new AverageMarketPrice { Price = "10000" };
            SetupHttpMessageHandler(HttpStatusCode.OK, JsonConvert.SerializeObject(averageMarketPrice));

            // Act
            var result = await _binanceService.GetAverageMarketPriceAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(averageMarketPrice.Price, result.Value.Price);
        }

        [Fact]
        public async Task GetAverageMarketPriceAsync_ApiCallFails_ReturnsFailure()
        {
            // Arrange
            SetupHttpMessageHandler(HttpStatusCode.BadRequest);

            // Act
            var result = await _binanceService.GetAverageMarketPriceAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(BinanceApiError, result.Error);
        }

        [Fact]
        public async Task GetOrderBookAsync_ApiCallIsSuccessful_ReturnSuccess()
        {
            // Arrange
            var orderBook = new BinanceOrderBook();
            SetupHttpMessageHandler(HttpStatusCode.OK, JsonConvert.SerializeObject(orderBook));

            // Act
            var result = await _binanceService.GetOrderBookAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(orderBook.Asks, result.Value.Asks);
        }

        [Fact]
        public async Task GetOrderBookAsync_ApiCallFails_ReturnFailure()
        {
            // Arrange
            SetupHttpMessageHandler(HttpStatusCode.BadRequest);

            // Act
            var result = await _binanceService.GetOrderBookAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(BinanceApiError, result.Error);
        }

        private void SetupHttpMessageHandler(HttpStatusCode statusCode, string content = "")
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content),
                });
        }
    }
}
