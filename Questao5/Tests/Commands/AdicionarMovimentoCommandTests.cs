using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Xunit;

namespace Questao5.Tests.Commands
{
    public class AdicionarMovimentoCommandTests
    {

        private readonly Mock<IAdicionarMovimentoRequest> _mockAdicionarMovimentoRequest;
        private readonly Mock<IBuscarContaCorrenteRequest> _mockContaCorrenteRequest;
        private readonly Mock<IBuscarRequisicaoRequest> _mockBuscarRequisicaoRequest;
        private readonly Mock<IAdicionarRequisicaoRequest> _mockAdicionarRequisicaoRequest;

        public AdicionarMovimentoCommandTests()
        {
            _mockAdicionarMovimentoRequest = new Mock<IAdicionarMovimentoRequest>();
            _mockContaCorrenteRequest = new Mock<IBuscarContaCorrenteRequest>();
            _mockBuscarRequisicaoRequest = new Mock<IBuscarRequisicaoRequest>();
            _mockAdicionarRequisicaoRequest = new Mock<IAdicionarRequisicaoRequest>();
        }

        [Fact]
        public async Task AdicionarMovimento_Executed_WithoutIdempotencia_ReturnSuccess()
        {
            var dataAtual = DateTime.Now;
            var contaCorrente = new BuscarContaCorrenteResponse
            {
                Ativo = true,
                Id = "1",
                Nome = "Henrique",
                Numero = 1
            };

            var movimento = new AdicionarMovimentoResponse("1");
            

            _mockContaCorrenteRequest.Setup(mcr => mcr.BuscarContaCorrente(It.IsAny<string>())).ReturnsAsync(contaCorrente);
            _mockAdicionarMovimentoRequest.Setup(mam => mam.AdicionarMovimento(It.IsAny<Movimento>())).ReturnsAsync(movimento);
            _mockAdicionarRequisicaoRequest.Setup(mam => mam.AdicionarRequisicao(It.IsAny<Idempotencia>())).ReturnsAsync("1");

            var command = new AdicionarMovimentoCommand
            {
                ContaCorrenteId = "1",
                IdRequisicao = "1",
                TipoMovimento = "C",
                Valor = 100
            };
            var handler = new AdicionarMovimentoCommandHandler(_mockAdicionarMovimentoRequest.Object, _mockContaCorrenteRequest.Object,
                _mockBuscarRequisicaoRequest.Object, _mockAdicionarRequisicaoRequest.Object);
            var result = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            _mockContaCorrenteRequest.Verify(pr => pr.BuscarContaCorrente(It.IsAny<string>()), Times.Once);
            _mockAdicionarMovimentoRequest.Verify(pr => pr.AdicionarMovimento(It.IsAny<Movimento>()), Times.Once);
            _mockAdicionarRequisicaoRequest.Verify(mam => mam.AdicionarRequisicao(It.IsAny<Idempotencia>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarMovimento_Executed_WithIdempotencia_ReturnSuccess()
        {
            var requisicao = new Idempotencia
            {
                Id = "1",
                Requisicao = "1",
                Resultado = "{\"id\": \"1\"}"
            };

            _mockBuscarRequisicaoRequest.Setup(mbr => mbr.ObterRequisicao(It.IsAny<string>())).ReturnsAsync(requisicao);

            var command = new AdicionarMovimentoCommand
            {
                ContaCorrenteId = "1",
                IdRequisicao = "1",
                TipoMovimento = "C",
                Valor = 100
            };
            var handler = new AdicionarMovimentoCommandHandler(_mockAdicionarMovimentoRequest.Object, _mockContaCorrenteRequest.Object,
                _mockBuscarRequisicaoRequest.Object, _mockAdicionarRequisicaoRequest.Object);
            var result = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            _mockBuscarRequisicaoRequest.Verify(mbr => mbr.ObterRequisicao(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarMovimento_Executed_When_InvalidAccount_ThrowInvalidAccountException()
        {

            var command = new AdicionarMovimentoCommand
            {
                ContaCorrenteId = "1",
                IdRequisicao = "1",
                TipoMovimento = "C",
                Valor = 100
            };
            var handler = new AdicionarMovimentoCommandHandler(_mockAdicionarMovimentoRequest.Object, _mockContaCorrenteRequest.Object,
                _mockBuscarRequisicaoRequest.Object, _mockAdicionarRequisicaoRequest.Object);

            await Assert.ThrowsAsync<InvalidAccountException>(async () => await handler.Handle(command, new CancellationToken()));
            _mockContaCorrenteRequest.Verify(pr => pr.BuscarContaCorrente(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarMovimento_Executed_When_InactiveAccount_ThrowInactiveAccountException()
        {

            var dataAtual = DateTime.Now;
            var contaCorrente = new BuscarContaCorrenteResponse
            {
                Ativo = false,
                Id = "1",
                Nome = "Henrique",
                Numero = 1
            };

            _mockContaCorrenteRequest.Setup(mcr => mcr.BuscarContaCorrente(It.IsAny<string>())).ReturnsAsync(contaCorrente);

            var command = new AdicionarMovimentoCommand
            {
                ContaCorrenteId = "1",
                IdRequisicao = "1",
                TipoMovimento = "C",
                Valor = 100
            };
            var handler = new AdicionarMovimentoCommandHandler(_mockAdicionarMovimentoRequest.Object, _mockContaCorrenteRequest.Object,
                _mockBuscarRequisicaoRequest.Object, _mockAdicionarRequisicaoRequest.Object);

            await Assert.ThrowsAsync<InactiveAccountException>(async () => await handler.Handle(command, new CancellationToken()));
            _mockContaCorrenteRequest.Verify(pr => pr.BuscarContaCorrente(It.IsAny<string>()), Times.Once);
        }
    }
}
