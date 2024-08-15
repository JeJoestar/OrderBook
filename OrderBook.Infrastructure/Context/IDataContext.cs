using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OrderBook.Domain.Domain;

namespace OrderBook.Infrastructure.Context
{
    public interface IDataContext : IDisposable
    {
        DbSet<Snapshot> Snapshots { get; set; }

        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        ChangeTracker ChangeTracker { get; }
    }
}
