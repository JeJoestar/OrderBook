using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrderBook.Infrastructure.Dto;

namespace OrderBook.Infrastructure.Domain
{
    public class BinanceOrderBook
    {
        [JsonProperty("lastUpdateId")]
        public long LastUpdateId { get; set; }

        [JsonProperty("bids")]
        public List<string[]> Bids { get; set; } = [];

        [JsonProperty("asks")]
        public List<string[]> Asks { get; set; } = [];

        public override bool Equals(object? obj)
        {
            var book = obj as BinanceOrderBook;

            if (book is null)
            {
                return false;
            }

            if (book.Bids.Count != Bids.Count)
            {
                return false;
            }

            if (book.Asks.Count != Asks.Count)
            {
                return false;
            }


            if (book.LastUpdateId != LastUpdateId)
            {
                return false;
            }

            return !book.Bids.Where((t, i) => !t.Equals(Bids[i])).Any()
                && !book.Asks.Where((t, i) => !t.Equals(Asks[i])).Any();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LastUpdateId, Bids, Asks);
        }
    }
}
