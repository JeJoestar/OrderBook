using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Infrastructure.Options
{
    public interface IConnectionOptions
    {
        public string ConnectionString { get; set; }
    }
}
