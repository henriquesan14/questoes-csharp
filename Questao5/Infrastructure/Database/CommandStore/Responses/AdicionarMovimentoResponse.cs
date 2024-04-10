namespace Questao5.Infrastructure.Database.CommandStore.Responses
{
    public class AdicionarMovimentoResponse
    {
        public AdicionarMovimentoResponse(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
