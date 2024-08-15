using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Infrastructure.Options
{
    public class ConnectionOptions : IConnectionOptions
    {
        public string ConnectionString { get; set; }
    }
}
