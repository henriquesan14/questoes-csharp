using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class BuscarRequisicaoRequest : IBuscarRequisicaoRequest
    {
        private readonly DatabaseConfig databaseConfig;

        public BuscarRequisicaoRequest(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<Idempotencia> ObterRequisicao(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            string query = @"
                SELECT chave_idempotencia AS Id,
                       requisicao AS Requisicao,
                       resultado AS Resultado
                FROM idempotencia
                WHERE requisicao = @Id
            ";

            var parameters = new
            {
                Id = id,
            };

            var response = await connection.QuerySingleOrDefaultAsync<Idempotencia>(query, parameters);

            return response;
        }
    }
}
