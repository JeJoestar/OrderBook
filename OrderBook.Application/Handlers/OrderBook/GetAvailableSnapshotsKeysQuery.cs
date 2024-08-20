using CSharpFunctionalExtensions;
using MediatR;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.Application.Handlers.OrderBook
{
    public class GetAvailableSnapshotsKeysQuery : IRequest<Result<List<DateTimeOffset>>>
    {
        public class Handler : IRequestHandler<GetAvailableSnapshotsKeysQuery, Result<List<DateTimeOffset>>>
        {
            private readonly ISnapshotService _snapshotService;

            public Handler(ISnapshotService snapshotService)
            {
                _snapshotService = snapshotService;
            }

            public async Task<Result<List<DateTimeOffset>>> Handle(GetAvailableSnapshotsKeysQuery request, CancellationToken cancellationToken)
            {
                var keys = _snapshotService.GetAvailableSnapshotsKeys();

                return Result.Success(keys);
            }
        }
    }
}
