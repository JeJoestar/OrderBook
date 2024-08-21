using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Dto;

namespace OrderBook.Infrastructure.Extensions
{
    public static class ToDtoExtensions
    {
        public static OrderBookDto ToDto(this BinanceOrderBook entity, DateTimeOffset retrievedAt)
        {
            return new OrderBookDto
            {
                RetrievedAt = retrievedAt,
                Asks = [.. entity.Asks
                        .Select(x => new AskBidDto(
                            Convert.ToDecimal(x[1]),
                            Convert.ToDecimal(x[0])))
                        .OrderByDescending(x => x.Price)],
                Bids = [.. entity.Bids
                        .Select(x => new AskBidDto(
                            Convert.ToDecimal(x[1]),
                            Convert.ToDecimal(x[0])))
                        .OrderByDescending(x => x.Price)],
            };
        }
    }
}
