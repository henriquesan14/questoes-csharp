using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class AdicionarRequisicaoRequest : IAdicionarRequisicaoRequest
    {
        private readonly DatabaseConfig databaseConfig;

        public AdicionarRequisicaoRequest(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<string> AdicionarRequisicao(Idempotencia idempotencia)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            string query = @"
            INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
            VALUES (@Id, @Requisicao, @Resultado);

            SELECT last_insert_rowid();
        ";

            var id = await connection.ExecuteScalarAsync<string>(query, idempotencia);
            return id;
        }
    }
}
