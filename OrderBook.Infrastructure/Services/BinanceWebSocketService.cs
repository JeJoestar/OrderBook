using Binance.Spot;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Extensions;
using OrderBook.Infrastructure.Hubs;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.Infrastructure.Services
{
    public class BinanceWebSocketService : IHostedService
    {
        private readonly MarketDataWebSocket _webSocket;
        private readonly ISnapshotService _snapshotService;
        private readonly IHubContext<TradeHub, ITradeHub> _hubContext;

        public BinanceWebSocketService(
            ISnapshotService snapshotService,
            IHubContext<TradeHub, ITradeHub> hubContext)
        {
            _webSocket = new MarketDataWebSocket("btceur@depth20");
            _snapshotService = snapshotService;
            _hubContext = hubContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _webSocket.OnMessageReceived(
                async (data) =>
                {
                    BinanceOrderBook? deserializedData = DeserializeData(data);
                    if (deserializedData is null)
                    {
                        return;
                    }

                    await _snapshotService.CreateSnapshotAsync(deserializedData, cancellationToken);
                    await _hubContext.Clients.All.SendTradeUpdate(deserializedData.ToDto());
                }, cancellationToken);

            await _webSocket.ConnectAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _webSocket.DisconnectAsync(cancellationToken);
        }

        private BinanceOrderBook? DeserializeData(string json)
        {
            return JsonConvert.DeserializeObject<BinanceOrderBook>(json);
        }
    }
}
