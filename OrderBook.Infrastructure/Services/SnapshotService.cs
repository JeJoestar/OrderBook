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

        public Task<Snapshot?> GetOrderBookSnapshotByDateAsync(DateTime key)
            => _context.Snapshots.FirstOrDefaultAsync(x => x.RetrievedAt == key);

        public async Task<PagedList<DateTimeOffset, DateTimeOffset>> GetAvailableSnapshotsKeysAsync(int pageSize, DateTimeOffset? pageNumber)
        {
            var data = await _context.Snapshots
                .OrderByDescending(x => x.RetrievedAt)
                .Where(s => s.RetrievedAt < (pageNumber ?? DateTimeOffset.MaxValue))
                .Take(pageSize + 1)
                .Select(s => s.RetrievedAt)
                .ToListAsync();

            return new PagedList<DateTimeOffset, DateTimeOffset>(data, data.Last(), pageSize);
        }

        public async Task CreateSnapshotAsync(BinanceOrderBook orderBook, DateTimeOffset retrievedAt, CancellationToken cancellationToken)
        {
            var entityToInsert = new Snapshot
            {
                RetrievedAt = retrievedAt,
                SnapshotJson = JsonConvert.SerializeObject(orderBook),
            };

            await _context.Snapshots.AddAsync(entityToInsert, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
