using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderBook.Domain.Domain;

namespace OrderBook.Infrastructure.Context.Configurations
{
    public class SnapshotConfiguration : IEntityTypeConfiguration<Snapshot>
    {
        public void Configure(EntityTypeBuilder<Snapshot> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
