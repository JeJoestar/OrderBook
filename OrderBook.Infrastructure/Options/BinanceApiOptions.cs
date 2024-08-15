using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Infrastructure.Options
{
    public class BinanceApiOptions : IBinanceApiOptions
    {
        public string BinanceApiBaseUrl { get; set; } = string.Empty;
    }
}
