using CSharpFunctionalExtensions;
using OrderBook.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Infrastructure.Services.Abstractions
{
    public interface IBinanceService
    {
        Task<Result<BinanceOrderBook>> GetOrderBookAsync();

        Task<Result<AverageMarketPrice>> GetAverageMarketPriceAsync();
    }
}
