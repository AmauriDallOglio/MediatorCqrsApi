using MassTransit.Mediator;
using MediatorCqrsApi.Aplicacao.DML.Notificacoes;
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


        [HttpGet("ObterTodas"), ActionName("ObterTodas")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterTodas()
        {
            var resultado = _mediator.Send(new ObterTodasNotificacaoRequest());
            return Ok(resultado);
        }


 

    }
}
