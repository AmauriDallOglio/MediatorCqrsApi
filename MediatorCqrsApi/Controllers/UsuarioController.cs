using MediatorCqrsApi.Aplicacao.DML.Empresas;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MediatorCqrsApi.Aplicacao.DML.Usuarios;

namespace MediatorCqrsApi.Controllers
{
    [Route("api/v1/Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Conexao"), ActionName("Conexao")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public string Conexao()
        {
            return "Ok";
        }



        [HttpPost("Inserir"), ActionName("Inserir")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Inserir([FromBody] UsuarioInserirRequest request)
        {
            var response = await _mediator.Send(request);
            if (response == null)
            {
                BadRequest(response);
            }
            return Ok(response);
        }
    }
}
