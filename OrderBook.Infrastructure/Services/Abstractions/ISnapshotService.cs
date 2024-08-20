using OrderBook.Domain.Domain;
using OrderBook.Infrastructure.Domain;

namespace OrderBook.Infrastructure.Services.Abstractions
{
    public interface ISnapshotService
    {

        Task<Snapshot?> GetOrderBookSnapshotByDateAsync(DateTime key);

        Task<List<DateTimeOffset>> GetAvailableSnapshotsKeysAsync();

        Task CreateSnapshotAsync(BinanceOrderBook orderBook, CancellationToken cancellationToken);
    }
}