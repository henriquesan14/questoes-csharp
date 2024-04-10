using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class AdicionarMovimentoRequest : IAdicionarMovimentoRequest
    {
        private readonly DatabaseConfig databaseConfig;

        public AdicionarMovimentoRequest(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<AdicionarMovimentoResponse> AdicionarMovimento(Movimento movimento)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            string query = @"
            INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES (@Id, @ContaCorrenteId, @DataMovimento, @TipoMovimento, @Valor);

            SELECT last_insert_rowid();
        ";

            var id = await connection.ExecuteScalarAsync<string>(query, movimento);
            return new AdicionarMovimentoResponse(id);
        }
    }
}
