using OrderBook.Infrastructure.Domain;

namespace OrderBook.Infrastructure.Services.Abstractions
{
    public interface ISnapshotService
    {
        Task CreateSnapshotAsync(BinanceOrderBook orderBook, CancellationToken cancellationToken);
    }
}