using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Application.Dto
{
    public sealed record AskBidDto (decimal Amount, decimal Price);
}
