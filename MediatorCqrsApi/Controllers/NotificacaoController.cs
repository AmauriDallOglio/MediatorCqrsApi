using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace MediatorCqrsApi.Controllers
{
    [Route("api/v1/Notificacao")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //var customer = await _mediator.Send(new FindCustomer(id));
            //if (customer is null)
            //{
            //    return NotFound();
            //}

            return Ok(null);
        }

    
    }
}
