using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class BuscarContaCorrenteRequest : IBuscarContaCorrenteRequest
    {
        private readonly DatabaseConfig databaseConfig;

        public BuscarContaCorrenteRequest(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<BuscarContaCorrenteResponse> BuscarContaCorrente(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            string query = @"
                SELECT idcontacorrente AS Id,
                       nome AS NomeTitular,
                       numero AS Numero,
                       ativo AS Ativo
                FROM contacorrente
                WHERE idcontacorrente = @ContaCorrenteId
            ";

            var parameters = new
            {
                ContaCorrenteId = id,
            };

            var response = await connection.QuerySingleOrDefaultAsync<BuscarContaCorrenteResponse>(query, parameters);

            return response;
        }
    }
}
