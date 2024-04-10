using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public interface IBuscarContaCorrenteRequest
    {
        Task<BuscarContaCorrenteResponse> BuscarContaCorrente(string id);
    }
}
