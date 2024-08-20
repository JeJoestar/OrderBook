using CSharpFunctionalExtensions;
using MediatR;
using Newtonsoft.Json;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Dto;
using OrderBook.Infrastructure.Services.Abstractions;
using OrderBook.Infrastructure.Extensions;

namespace OrderBook.Application.Handlers.OrderBook
{
    public class GetOrderBookSnapshotByDateQuery : IRequest<Result<OrderBookDto>>
    {
        public DateTime Key { get; set; }

        public class Handler : IRequestHandler<GetOrderBookSnapshotByDateQuery, Result<OrderBookDto>>
        {
            private readonly ISnapshotService _snapshotService;

            public Handler(ISnapshotService snapshotService)
            {
                _snapshotService = snapshotService;
            }

            public async Task<Result<OrderBookDto>> Handle(GetOrderBookSnapshotByDateQuery request, CancellationToken cancellationToken)
            {
                var snapshot = await _snapshotService.GetOrderBookSnapshotByDate(request.Key);

                if (snapshot is null)
                {
                    return Result.Failure<OrderBookDto>("Failed to extract a snapshot for this date");
                }

                var binanceOrderBook = JsonConvert.DeserializeObject<BinanceOrderBook>(snapshot.SnapshotJson);

                if (binanceOrderBook is null)
                {
                    return Result.Failure<OrderBookDto>("Failed to read a snapshot returned from database");
                }

                return Result.Success(binanceOrderBook.ToDto());
            }
        }
    }
}
