using OrderBook.Infrastructure.Dto;

namespace OrderBook.Infrastructure.Hubs
{
    public interface ITradeHub
    {
        Task SendTradeUpdate(OrderBookDto orderBook);
    }
}
