using Microsoft.EntityFrameworkCore;
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

        public async Task<Snapshot?> GetOrderBookSnapshotByDate(DateTime key)
        {
            return await _context.Snapshots.FirstOrDefaultAsync(x => x.RetrievedAt == key);
        }

        public async Task<List<DateTimeOffset>> GetAvailableSnapshotsKeysAsync()
        {
            return await _context.Snapshots.Select(s => s.RetrievedAt).ToListAsync();
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
