using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Domain.Domain
{
    public class Snapshot
    {
        public int Id { get; set; }

        public string SnapshotJson { get; set; } = null!;

        public DateTimeOffset RetrievedAt { get; set; }
    }
}
