using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(AdicionarMovimentoResponse), 200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AdicionarMovimentoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
