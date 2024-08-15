using CSharpFunctionalExtensions;
using MediatR;
using OrderBook.Application.Dto;
using OrderBook.Infrastructure.Domain;
using OrderBook.Infrastructure.Services.Abstractions;


namespace OrderBook.Application.Handlers.OrderBook
{
    public class GetCurrentOrderBookQuery : IRequest<Result<OrderBookDto>>
    {
        public class Handler : IRequestHandler<GetCurrentOrderBookQuery, Result<OrderBookDto>>
        {
            private readonly IBinanceService _binanceService;
            private readonly ISnapshotService _snapshotService;

            public Handler(IBinanceService binanceService, ISnapshotService snapshotService)
            {
                _binanceService = binanceService;
                _snapshotService = snapshotService;
            }

            public async Task<Result<OrderBookDto>> Handle(GetCurrentOrderBookQuery request, CancellationToken cancellationToken)
            {
                Result<BinanceOrderBook> orderBookResult =
                    await _binanceService.GetOrderBookAsync(cancellationToken);

                if (orderBookResult.IsFailure)
                {
                    return Result.Failure<OrderBookDto>(orderBookResult.Error);
                }

                Result<AverageMarketPrice> averagePriceResult = 
                    await _binanceService.GetAverageMarketPriceAsync(cancellationToken);

                if (averagePriceResult.IsFailure)
                {
                    return Result.Failure<OrderBookDto>(averagePriceResult.Error);
                }

                await _snapshotService.CreateSnapshotAsync(orderBookResult.Value, cancellationToken);

                return Result.Success(new OrderBookDto
                {
                    Asks = [.. orderBookResult.Value.Asks
                        .Select(x => new AskBidDto(
                            Convert.ToDecimal(x[1]),
                            Convert.ToDecimal(x[0])))
                        .OrderByDescending(x => x.Price)],
                    Bids = [.. orderBookResult.Value.Bids
                        .Select(x => new AskBidDto(
                            Convert.ToDecimal(x[1]),
                            Convert.ToDecimal(x[0])))
                        .OrderByDescending(x => x.Price)],
                    AveragePrice = Convert.ToDecimal(averagePriceResult.Value.Price),
                });
            }
        }
    }
}
