using OrderBook.Domain.Domain;
using OrderBook.Infrastructure.Domain;

namespace OrderBook.Infrastructure.Services.Abstractions
{
    public interface ISnapshotService
    {

        Task<Snapshot?> GetOrderBookSnapshotByDateAsync(DateTime key);

        Task<PagedList<DateTimeOffset, DateTimeOffset?>> GetAvailableSnapshotsKeysAsync(int pageSize, DateTimeOffset? pageNumber);

        Task CreateSnapshotAsync(BinanceOrderBook orderBook, DateTimeOffset retrievedAt, CancellationToken cancellationToken);
    }
}