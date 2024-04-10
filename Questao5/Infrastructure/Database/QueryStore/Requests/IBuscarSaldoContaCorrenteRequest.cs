using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public interface IBuscarSaldoContaCorrenteRequest
    {
        Task<SaldoContaCorrenteResponse> ObterSaldoConta(string id);
    }
}
