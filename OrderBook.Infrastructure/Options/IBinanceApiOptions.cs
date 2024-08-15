using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Infrastructure.Options
{
    public interface IBinanceApiOptions
    {
        public string BinanceApiBaseUrl { get; set; }
    }
}
