namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class SaldoContaCorrenteResponse
    {
        public string IdDaConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal ValorSaldoAtual { get; set; }
    }
}
