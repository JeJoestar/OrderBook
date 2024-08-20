using Microsoft.Extensions.Options;
using OrderBook.Application.Handlers.OrderBook;
using OrderBook.Infrastructure.Context;
using OrderBook.Infrastructure.Options;
using OrderBook.Infrastructure.Services;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.WebAPI.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetSection(nameof(ConnectionOptions)));
        }

        public static void ConfigureApi(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(typeof(GetOrderBookSnapshotByDateQuery).Assembly);
            });

            builder.Services.AddEndpointsApiExplorer();
        }

        public static void ConfigureInAppServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IConnectionOptions>(sp => sp.GetRequiredService<IOptions<ConnectionOptions>>().Value);
            builder.Services.AddTransient<ISnapshotService, SnapshotService>();
            builder.Services.AddTransient<IDataContext, ApplicationDbContext>();
            builder.Services.AddHostedService<BinanceWebSocketService>();
            builder.Services.AddHttpClient();
            builder.Services.AddSignalR();
        }
    }
}
