using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class BuscarSaldoContaCorrenteRequest : IBuscarSaldoContaCorrenteRequest
    {
        private readonly DatabaseConfig databaseConfig;

        public BuscarSaldoContaCorrenteRequest(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<SaldoContaCorrenteResponse> ObterSaldoConta(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            string query = @"
                SELECT cc.idcontacorrente AS IdDaConta,
                       cc.nome AS NomeTitular,
                       datetime('now') AS DataHoraConsulta,
                       SUM(CASE WHEN m.tipomovimento = @Credito THEN m.valor ELSE 0 END) -
                       SUM(CASE WHEN m.tipomovimento = @Debito THEN m.valor ELSE 0 END) AS ValorSaldoAtual
                FROM contacorrente cc
                LEFT JOIN movimento m ON cc.idcontacorrente = m.idcontacorrente
                WHERE cc.idcontacorrente = @ContaCorrenteId
                GROUP BY cc.idcontacorrente, cc.nome;
            ";

            var parameters = new
            {
                ContaCorrenteId = id,
                Credito = TipoMovimentoEnum.Credito.GetDescription(),
                Debito = TipoMovimentoEnum.Debito.GetDescription()
            };

            var response = await connection.QuerySingleOrDefaultAsync<SaldoContaCorrenteResponse>(query, parameters);

            return response;
        }
    }
}
