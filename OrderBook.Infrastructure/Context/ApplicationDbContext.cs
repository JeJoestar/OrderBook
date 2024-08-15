using Microsoft.EntityFrameworkCore;
using OrderBook.Domain.Domain;
using OrderBook.Infrastructure.Options;

namespace OrderBook.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IDataContext
    {
        private readonly IConnectionOptions _options;

        public DbSet<Snapshot> Snapshots { get; set; }

        public ApplicationDbContext(IConnectionOptions options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options?.ConnectionString != null)
            {
                optionsBuilder.UseNpgsql(_options.ConnectionString);
            }
            else
            {
                optionsBuilder.UseNpgsql();
            }
        }
    }
}
