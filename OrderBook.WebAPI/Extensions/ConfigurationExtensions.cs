using Microsoft.Extensions.Options;
using OrderBook.Application.Handlers.OrderBook;
using OrderBook.Infrastructure.Options;
using OrderBook.Infrastructure.Services;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.WebAPI.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<BinanceApiOptions>(builder.Configuration.GetSection(nameof(BinanceApiOptions)));
        }

        public static void ConfigureApi(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(typeof(GetCurrentOrderBookQuery).Assembly);
            });

            builder.Services.AddEndpointsApiExplorer();
        }

        public static void ConfigureInAppServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IBinanceApiOptions>(sp => sp.GetRequiredService<IOptions<BinanceApiOptions>>().Value);
            builder.Services.AddTransient<IBinanceService, BinanceService>();
            builder.Services.AddHttpClient();
        }
    }
}
