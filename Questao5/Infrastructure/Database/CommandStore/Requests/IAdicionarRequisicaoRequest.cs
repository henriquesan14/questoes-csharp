using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public interface IAdicionarRequisicaoRequest
    {
        Task<string> AdicionarRequisicao(Idempotencia idempotencia);
    }
}
