using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

    }
}
