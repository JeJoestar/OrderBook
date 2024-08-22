using Moq;
using Newtonsoft.Json;
using OrderBook.Application.Handlers.OrderBook;
using OrderBook.Domain.Domain;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.Tests.CommandsAndHandlers
{
    public class GetOrderBookSnapshotByDateQueryHandlerTests
    {
        private readonly Mock<ISnapshotService> _mockSnapshotService;
        private readonly GetOrderBookSnapshotByDateQuery.Handler _handler;

        public GetOrderBookSnapshotByDateQueryHandlerTests()
        {
            _mockSnapshotService = new Mock<ISnapshotService>();
            _handler = new GetOrderBookSnapshotByDateQuery.Handler(_mockSnapshotService.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenSnapshotIsValid()
        {
            // Arrange
            var requestDate = DateTime.UtcNow;
            var snapshotJson = JsonConvert.SerializeObject(new BinanceOrderBook());
            var snapshot = new Snapshot
            {
                RetrievedAt = requestDate,
                SnapshotJson = snapshotJson
            };

            _mockSnapshotService
                .Setup(service => service.GetOrderBookSnapshotByDateAsync(requestDate))
                .ReturnsAsync(snapshot);

            var query = new GetOrderBookSnapshotByDateQuery { Key = requestDate };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockSnapshotService.Verify(service => service.GetOrderBookSnapshotByDateAsync(requestDate), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenSnapshotIsNull()
        {
            // Arrange
            var requestDate = DateTime.UtcNow;

            _mockSnapshotService
                .Setup(service => service.GetOrderBookSnapshotByDateAsync(requestDate))
                .ReturnsAsync(default(Snapshot?));

            var query = new GetOrderBookSnapshotByDateQuery { Key = requestDate };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Failed to extract a snapshot for this date", result.Error);
            _mockSnapshotService.Verify(service => service.GetOrderBookSnapshotByDateAsync(requestDate), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenSnapshotJsonIsInvalid()
        {
            // Arrange
            var requestDate = DateTime.UtcNow;
            var invalidJson = "invalid_json";
            var snapshot = new Snapshot
            {
                RetrievedAt = requestDate,
                SnapshotJson = invalidJson
            };

            _mockSnapshotService
                .Setup(service => service.GetOrderBookSnapshotByDateAsync(requestDate))
                .ReturnsAsync(snapshot);

            var query = new GetOrderBookSnapshotByDateQuery { Key = requestDate };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Failed to read a snapshot returned from database", result.Error);
            _mockSnapshotService.Verify(service => service.GetOrderBookSnapshotByDateAsync(requestDate), Times.Once);
        }
    }
}
