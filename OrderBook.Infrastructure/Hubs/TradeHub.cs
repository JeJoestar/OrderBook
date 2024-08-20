using Microsoft.AspNetCore.SignalR;

namespace OrderBook.Infrastructure.Hubs
{
    public class TradeHub : Hub<ITradeHub>
    {
    }
}
