using Moq;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Xunit;

namespace Questao5.Tests.Queries
{
    public class BuscarSaldoContaCorrenteQueryTests
    {
        private readonly Mock<IBuscarSaldoContaCorrenteRequest> _mockSaldoContaCorrenteRequest;
        private readonly Mock<IBuscarContaCorrenteRequest> _mockContaCorrenteRequest;

        public BuscarSaldoContaCorrenteQueryTests()
        {
            _mockSaldoContaCorrenteRequest = new Mock<IBuscarSaldoContaCorrenteRequest>();
            _mockContaCorrenteRequest =  new Mock<IBuscarContaCorrenteRequest>();
        }

        [Fact]
        public async Task BuscarSaldoContaCorrente_Executed_ReturnSuccess()
        {
            var dataAtual = DateTime.Now;
            var contaCorrente = new BuscarContaCorrenteResponse {
                Ativo = true,
                Id = "1",
                Nome = "Henrique",
                Numero = 1
            };
            var saldo = new SaldoContaCorrenteResponse {
                DataHoraConsulta = dataAtual,
                IdDaConta = "1",
                NomeTitular = "Henrique",
                ValorSaldoAtual = 1000
            };
            _mockContaCorrenteRequest.Setup(mcr => mcr.BuscarContaCorrente(It.IsAny<string>())).ReturnsAsync(contaCorrente);
            _mockSaldoContaCorrenteRequest.Setup(mr => mr.ObterSaldoConta(It.IsAny<string>())).ReturnsAsync(saldo);

            var query = new BuscarSaldoContaCorrenteQuery("123");
            var handler = new BuscarSaldoContaCorrenteQueryHandler(_mockSaldoContaCorrenteRequest.Object, _mockContaCorrenteRequest.Object);
            var result = await handler.Handle(query, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(1000, result.ValorSaldoAtual);
            _mockContaCorrenteRequest.Verify(pr => pr.BuscarContaCorrente(It.IsAny<string>()), Times.Once);
            _mockSaldoContaCorrenteRequest.Verify(pr => pr.ObterSaldoConta(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task BuscarSaldoContaCorrente_Executed_When_InvalidAccount_ThrowInvalidAccountException()
        {

            var query = new BuscarSaldoContaCorrenteQuery("123");
            var handler = new BuscarSaldoContaCorrenteQueryHandler(_mockSaldoContaCorrenteRequest.Object, _mockContaCorrenteRequest.Object);

            await Assert.ThrowsAsync<InvalidAccountException>(async () => await handler.Handle(query, new CancellationToken()));
            _mockContaCorrenteRequest.Verify(pr => pr.BuscarContaCorrente(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task BuscarSaldoContaCorrente_Executed_When_InactiveAccount_ThrowInactiveAccountException()
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

            var query = new BuscarSaldoContaCorrenteQuery("123");
            var handler = new BuscarSaldoContaCorrenteQueryHandler(_mockSaldoContaCorrenteRequest.Object, _mockContaCorrenteRequest.Object);

            await Assert.ThrowsAsync<InactiveAccountException>(async () => await handler.Handle(query, new CancellationToken()));
            _mockContaCorrenteRequest.Verify(pr => pr.BuscarContaCorrente(It.IsAny<string>()), Times.Once);
        }
    }
}
