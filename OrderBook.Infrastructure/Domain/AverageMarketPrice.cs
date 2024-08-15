using Newtonsoft.Json;

namespace OrderBook.Infrastructure.Domain
{
    public class AverageMarketPrice
    {
        [JsonProperty("mins")]
        public int Mins { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; } = null!;

        [JsonProperty("closeTime")]
        public long CloseTime { get; set; }
    }
}
