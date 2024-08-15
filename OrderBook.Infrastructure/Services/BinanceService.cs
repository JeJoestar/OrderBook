using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Newtonsoft.Json;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Options;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.Infrastructure.Services
{
    public class BinanceService : IBinanceService
    {
        private readonly IBinanceApiOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string DepthChartUrl = "depth?symbol=BTCEUR";
        private const string AveragePriceUrl = "avgPrice?symbol=BTCEUR";

        public BinanceService(IBinanceApiOptions options, IHttpClientFactory httpClientFactory)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result<AverageMarketPrice>> GetAverageMarketPriceAsync()
        {
            var result = await SendRequest<AverageMarketPrice>(_options.BinanceApiBaseUrl + AveragePriceUrl);

            if (result.IsFailure)
            {
                return Result.Failure<AverageMarketPrice>(result.Error);
            }

            return Result.Success(result.Value ?? new AverageMarketPrice());
        }

        public async Task<Result<BinanceOrderBook>> GetOrderBookAsync()
        {
            var result = await SendRequest<BinanceOrderBook>(_options.BinanceApiBaseUrl + DepthChartUrl);

            if (result.IsFailure)
            {
                return Result.Failure<BinanceOrderBook>(result.Error);
            }

            return Result.Success(result.Value ?? new BinanceOrderBook());
        }

        private async Task<Result<TResponse?>> SendRequest<TResponse>(string url)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return Result.Success(JsonConvert.DeserializeObject<TResponse>(responseBody));
            }

            return Result.Failure<TResponse?>("Failed to extract the order book from Binance.");
        }
    }
}
