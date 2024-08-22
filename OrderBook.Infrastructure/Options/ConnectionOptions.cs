namespace OrderBook.Infrastructure.Options
{
    public class ConnectionOptions : IConnectionOptions
    {
        public string ConnectionString { get; set; } = null!;
    }
}
