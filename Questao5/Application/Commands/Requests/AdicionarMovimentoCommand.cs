using MediatR;
using Questao5.Infrastructure.Database.CommandStore.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class AdicionarMovimentoCommand : IRequest<AdicionarMovimentoResponse>
    {
        public string IdRequisicao { get; set; }
        public string ContaCorrenteId { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
