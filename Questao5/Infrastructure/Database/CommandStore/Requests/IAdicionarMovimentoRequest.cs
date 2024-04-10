using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Responses;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public interface IAdicionarMovimentoRequest
    {
        Task<AdicionarMovimentoResponse> AdicionarMovimento(Movimento movimento);
    }
}
