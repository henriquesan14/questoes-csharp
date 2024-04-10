using MediatR;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class BuscarSaldoContaCorrenteQuery : IRequest<SaldoContaCorrenteResponse>
    {
        public BuscarSaldoContaCorrenteQuery(string idDaConta)
        {
            IdDaConta = idDaConta;
        }

        public string IdDaConta { get; set; }
    }
}
