namespace OrderBook.Infrastructure.Dto
{
    public class OrderBookDto
    {
        public DateTimeOffset RetrievedAt { get; set; }

        public List<AskBidDto> Bids { get; set; } = new ();

        public List<AskBidDto> Asks { get; set; } = new ();
    }
}
