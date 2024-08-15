using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderBook.Application.Dto;
using OrderBook.Application.Handlers.OrderBook;

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
        public async Task<IActionResult> GetCurrentOrderBook()
        {
            Result<OrderBookDto> result = await _mediator.Send(new GetCurrentOrderBookQuery());

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
