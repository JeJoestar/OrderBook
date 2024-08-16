using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using OrderBook.Domain.Domain;
using OrderBook.Infrastructure.Context;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Services;
using Xunit;

namespace OrderBook.Tests.Infrastructure.Services
{
    public class SnapshotServiceTests
    {
        private readonly Mock<IDataContext> _mockContext;
        private readonly SnapshotService _snapshotService;

        public SnapshotServiceTests()
        {
            _mockContext = new Mock<IDataContext>();
            _snapshotService = new SnapshotService(_mockContext.Object);
        }

        [Fact]
        public async Task CreateSnapshotAsync_Should_AddSnapshotToContext()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Snapshot>>();
            _mockContext.Setup(m => m.Snapshots).Returns(mockDbSet.Object);

            var orderBook = new BinanceOrderBook();
            var cancellationToken = CancellationToken.None;

            // Act
            await _snapshotService.CreateSnapshotAsync(orderBook, cancellationToken);

            // Assert
            mockDbSet.Verify(m => m.AddAsync(It.Is<Snapshot>(s =>
                s.SnapshotJson == JsonConvert.SerializeObject(orderBook) &&
                s.RetrievedAt <= DateTime.UtcNow), cancellationToken), Times.Once);

            _mockContext.Verify(m => m.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task CreateSnapshotAsync_Should_CallSaveChangesAsync()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Snapshot>>();
            _mockContext.Setup(m => m.Snapshots).Returns(mockDbSet.Object);

            var orderBook = new BinanceOrderBook();
            var cancellationToken = CancellationToken.None;

            // Act
            await _snapshotService.CreateSnapshotAsync(orderBook, cancellationToken);

            // Assert
            _mockContext.Verify(m => m.SaveChangesAsync(cancellationToken), Times.Once);
        }
    }
}