using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Application.Dto
{
    public class OrderBookDto
    {
        public List<AskBidDto> Bids { get; set; } = new ();

        public List<AskBidDto> Asks { get; set; } = new ();

        public decimal AveragePrice { get; set; } 
    }
}
