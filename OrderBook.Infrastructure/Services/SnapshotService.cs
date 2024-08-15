using Newtonsoft.Json;
using OrderBook.Domain.Domain;
using OrderBook.Infrastructure.Context;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.Infrastructure.Services
{
    public class SnapshotService : ISnapshotService
    {
        private readonly IDataContext _context;

        public SnapshotService(IDataContext dbContext)
        {
            _context = dbContext;
        }

        public async Task CreateSnapshotAsync(BinanceOrderBook orderBook, CancellationToken cancellationToken)
        {
            var entityToInsert = new Snapshot
            {
                RetrievedAt = DateTime.UtcNow,
                SnapshotJson = JsonConvert.SerializeObject(orderBook),
            };

            await _context.Snapshots.AddAsync(entityToInsert, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
