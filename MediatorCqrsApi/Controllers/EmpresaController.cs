using MediatorCqrsApi.Aplicacao.DML.Empresas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediatorCqrsApi.Controllers
{
    [Route("api/v1/Empresa")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmpresaController(IMediator mediator)
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
        public async Task<IActionResult> Inserir([FromBody] EmpresaInserirRequest request)
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
