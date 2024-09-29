using MediatorCqrsApi.Aplicacao.DML.Notificacoes;
using MediatorCqrsApi.Aplicacao.Util;
using MediatR;
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
            ResultadoOperacao<ObterTodasNotificacaoResponse> resultado = await _mediator.Send(new ObterTodasNotificacaoRequest());
            if (resultado == null)
            {
                BadRequest(resultado);
            }
            return Ok(resultado);
        }


 

    }
}
