using MediatR;
using Newtonsoft.Json;
using Questao5.Domain.Entities;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Commands.Requests
{
    public class AdicionarMovimentoCommandHandler : IRequestHandler<AdicionarMovimentoCommand, AdicionarMovimentoResponse>
    {
        private readonly IAdicionarMovimentoRequest _request;
        private readonly IBuscarContaCorrenteRequest _contaCorrenteRequest;
        private readonly IBuscarRequisicaoRequest _buscarRequisicaoRequest;
        private readonly IAdicionarRequisicaoRequest _adicionarRequisicaoRequest;

        public AdicionarMovimentoCommandHandler(IAdicionarMovimentoRequest request, IBuscarContaCorrenteRequest contaCorrenteRequest, IBuscarRequisicaoRequest buscarRequisicaoRequest, IAdicionarRequisicaoRequest adicionarRequisicaoRequest)
        {
            _request = request;
            _contaCorrenteRequest = contaCorrenteRequest;
            _buscarRequisicaoRequest = buscarRequisicaoRequest;
            _adicionarRequisicaoRequest = adicionarRequisicaoRequest;
        }

        public async Task<AdicionarMovimentoResponse> Handle(AdicionarMovimentoCommand request, CancellationToken cancellationToken)
        {
            var requisicao = await _buscarRequisicaoRequest.ObterRequisicao(request.IdRequisicao);
            if(requisicao != null)
            {
                var resultado = JsonConvert.DeserializeObject<AdicionarMovimentoResponse>(requisicao.Resultado);
                return resultado;
            }
            var conta = await _contaCorrenteRequest.BuscarContaCorrente(request.ContaCorrenteId);
            if (conta == null)
            {
                throw new InvalidAccountException("Apenas contas correntes cadastradas podem consultar o saldo");
            }
            if (!conta.Ativo)
            {
                throw new InactiveAccountException("Apenas contas correntes ativas podem consultar o saldo");
            }
            var movimento = new Movimento(request.ContaCorrenteId, request.TipoMovimento, request.Valor);
            var result = await _request.AdicionarMovimento(movimento);
            var resultString = JsonConvert.SerializeObject(result);
            var idempotencia = new Idempotencia(request.IdRequisicao, resultString);
            await _adicionarRequisicaoRequest.AdicionarRequisicao(idempotencia);
            return result;
        }
    }
}
