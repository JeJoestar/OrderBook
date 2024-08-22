using CSharpFunctionalExtensions;
using MediatR;
using OrderBook.Infrastructure.Services.Abstractions;

namespace OrderBook.Application.Handlers.OrderBook
{
    public class GetAvailableSnapshotsKeysQuery : IRequest<Result<PagedList<DateTimeOffset, DateTimeOffset>>>
    {
        public int PageSize { get; set; }

        public DateTimeOffset? PageNumber { get; set; }

        public class Handler : IRequestHandler<GetAvailableSnapshotsKeysQuery, Result<PagedList<DateTimeOffset, DateTimeOffset>>>
        {
            private readonly ISnapshotService _snapshotService;

            public Handler(ISnapshotService snapshotService)
            {
                _snapshotService = snapshotService;
            }

            public async Task<Result<PagedList<DateTimeOffset, DateTimeOffset>>> Handle(GetAvailableSnapshotsKeysQuery request, CancellationToken cancellationToken)
            {
                var keys = await _snapshotService.GetAvailableSnapshotsKeysAsync(request.PageSize, request.PageNumber);

                return Result.Success(keys);
            }
        }
    }
}
