using Moq;
using OrderBook.Application.Handlers.OrderBook;
using OrderBook.Infrastructure.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Tests.CommandsAndHandlers
{
    public class GetAvailableSnapshotsKeysQueryHandlerTests
    {
        private readonly Mock<ISnapshotService> _mockSnapshotService;
        private readonly GetAvailableSnapshotsKeysQuery.Handler _handler;

        public GetAvailableSnapshotsKeysQueryHandlerTests()
        {
            _mockSnapshotService = new Mock<ISnapshotService>();
            _handler = new GetAvailableSnapshotsKeysQuery.Handler(_mockSnapshotService.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenSnapshotsExist()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = DateTimeOffset.UtcNow.AddDays(-1);
            var pagedList = new PagedList<DateTimeOffset, DateTimeOffset?>(
                new List<DateTimeOffset> { DateTimeOffset.UtcNow },
                pageNumber,
                pageSize
            );

            _mockSnapshotService
                .Setup(service => service.GetAvailableSnapshotsKeysAsync(pageSize, pageNumber))
                .ReturnsAsync(pagedList);

            var query = new GetAvailableSnapshotsKeysQuery { PageSize = pageSize, PageNumber = pageNumber };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(pagedList, result.Value);
            _mockSnapshotService.Verify(service => service.GetAvailableSnapshotsKeysAsync(pageSize, pageNumber), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WithEmptyList_WhenNoSnapshotsExist()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = DateTimeOffset.UtcNow.AddDays(-1);
            var pagedList = new PagedList<DateTimeOffset, DateTimeOffset?>(new List<DateTimeOffset>(), pageNumber, pageSize);

            _mockSnapshotService
                .Setup(service => service.GetAvailableSnapshotsKeysAsync(pageSize, pageNumber))
                .ReturnsAsync(pagedList);

            var query = new GetAvailableSnapshotsKeysQuery { PageSize = pageSize, PageNumber = pageNumber };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value.Items);
            _mockSnapshotService.Verify(service => service.GetAvailableSnapshotsKeysAsync(pageSize, pageNumber), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WithNull_WhenSnapshotServiceReturnsNull()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = DateTimeOffset.UtcNow.AddDays(-1);

            _mockSnapshotService
                .Setup(service => service.GetAvailableSnapshotsKeysAsync(pageSize, pageNumber))
                .ReturnsAsync(new PagedList<DateTimeOffset, DateTimeOffset?>([], null, 10));

            var query = new GetAvailableSnapshotsKeysQuery { PageSize = pageSize, PageNumber = pageNumber };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Value);
            _mockSnapshotService.Verify(service => service.GetAvailableSnapshotsKeysAsync(pageSize, pageNumber), Times.Once);
        }
    }
}
