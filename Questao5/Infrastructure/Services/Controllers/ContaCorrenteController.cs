using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController :ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(SaldoContaCorrenteResponse), 200)]
        [ProducesResponseType(400)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaldo(string id)
        {
            var query = new BuscarSaldoContaCorrenteQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
