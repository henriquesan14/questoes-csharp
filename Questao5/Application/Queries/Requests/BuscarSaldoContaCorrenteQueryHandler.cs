using MediatR;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class BuscarSaldoContaCorrenteQueryHandler : IRequestHandler<BuscarSaldoContaCorrenteQuery, SaldoContaCorrenteResponse>
    {
        private readonly IBuscarSaldoContaCorrenteRequest _request;
        private readonly IBuscarContaCorrenteRequest _contaCorrenteRequest;

        public BuscarSaldoContaCorrenteQueryHandler(IBuscarSaldoContaCorrenteRequest request, IBuscarContaCorrenteRequest contaCorrenteRequest)
        {
            _request = request;
            _contaCorrenteRequest = contaCorrenteRequest;
        }

        public async Task<SaldoContaCorrenteResponse> Handle(BuscarSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaCorrenteRequest.BuscarContaCorrente(request.IdDaConta);
            if(conta == null)
            {
                throw new InvalidAccountException("Apenas contas correntes cadastradas podem consultar o saldo");
            }
            if (!conta.Ativo)
            {
                throw new InactiveAccountException("Apenas contas correntes ativas podem consultar o saldo");
            }
            var result = await _request.ObterSaldoConta(request.IdDaConta);
            return result;
        }
    }
}
