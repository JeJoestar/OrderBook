using OrderBook.Domain.Domain;
using OrderBook.Infrastructure.Domain;

namespace OrderBook.Infrastructure.Services.Abstractions
{
    public interface ISnapshotService
    {
        Snapshot? GetOrderBookSnapshotByDate(DateTime key);

        List<DateTimeOffset> GetAvailableSnapshotsKeys();

        Task CreateSnapshotAsync(BinanceOrderBook orderBook, CancellationToken cancellationToken);
    }
}