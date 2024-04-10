using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public interface IBuscarRequisicaoRequest
    {
        Task<Idempotencia> ObterRequisicao(string id);
    }
}
