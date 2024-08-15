using Microsoft.EntityFrameworkCore;
using OrderBook.Infrastructure.Context;

namespace OrderBook.WebAPI.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task RunMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            IDataContext db = scope.ServiceProvider.GetRequiredService<IDataContext>();
            await db.Database.MigrateAsync();
        }
    }
}
