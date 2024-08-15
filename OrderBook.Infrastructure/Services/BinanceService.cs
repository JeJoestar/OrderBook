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

        public async Task<Result<AverageMarketPrice>> GetAverageMarketPriceAsync(CancellationToken cancellationToken)
        {
            var result = await SendRequestAsync<AverageMarketPrice>(
                _options.BinanceApiBaseUrl + AveragePriceUrl,
                cancellationToken);

            if (result.IsFailure)
            {
                return Result.Failure<AverageMarketPrice>(result.Error);
            }

            return Result.Success(result.Value ?? new AverageMarketPrice());
        }

        public async Task<Result<BinanceOrderBook>> GetOrderBookAsync(CancellationToken cancellationToken)
        {
            var result = await SendRequestAsync<BinanceOrderBook>(
                _options.BinanceApiBaseUrl + DepthChartUrl,
                cancellationToken);

            if (result.IsFailure)
            {
                return Result.Failure<BinanceOrderBook>(result.Error);
            }

            return Result.Success(result.Value ?? new BinanceOrderBook());
        }

        private async Task<Result<TResponse?>> SendRequestAsync<TResponse>(string url, CancellationToken cancellationToken)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                return Result.Success(JsonConvert.DeserializeObject<TResponse>(responseBody));
            }

            return Result.Failure<TResponse?>("Failed to extract the order book from Binance.");
        }
    }
}
