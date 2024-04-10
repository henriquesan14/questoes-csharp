namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public Idempotencia(string requisicao, string resultado)
        {
            Id = Guid.NewGuid().ToString();
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public Idempotencia()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }
    }
}
