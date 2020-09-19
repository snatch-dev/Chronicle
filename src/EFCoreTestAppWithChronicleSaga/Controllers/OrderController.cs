using System;
using System.Threading.Tasks;
using Chronicle;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EFCoreTestApp.Commands;
using EFCoreTestApp.Events;
using EFCoreTestApp.Persistence;
using EFCoreTestApp.DTO;

namespace EFCoreTestApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // [HttpPost, Route("{id:guid}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto)
        {
            // [FromRoute] Guid id
            var request = new CreateOrder(dto.OrderId, dto.CustomerId, dto.ParcelId);
            var result = await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("created")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OrderCreated([FromBody] OrderCreatedDTO dto)
        {
            // [FromRoute] Guid id
            var notification = new OrderCreated(dto.OrderId);
            await _mediator.Publish(notification);
            return Ok();
        }

    }
}
