using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderBook.Application.Handlers.OrderBook;
using OrderBook.Infrastructure.Dto;

namespace OrderBook.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(OrderBookDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderBookSnaphostByDate(DateTime key)
        {
            Result<OrderBookDto> result = await _mediator.Send(new GetOrderBookSnapshotByDateQuery()
            {
                Key = key,
            });

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("date-keys")]
        [ProducesResponseType(typeof(PagedList<DateTimeOffset, DateTimeOffset?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailabeSnapshotsKeys([FromQuery] int pageSize, [FromQuery] DateTimeOffset? pageNumber = null)
        {
            Result<PagedList<DateTimeOffset, DateTimeOffset?>> result = await _mediator.Send(new GetAvailableSnapshotsKeysQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            });

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
