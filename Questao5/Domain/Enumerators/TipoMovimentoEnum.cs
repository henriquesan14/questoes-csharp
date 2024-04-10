using System.ComponentModel;

namespace Questao5.Domain.Enumerators
{
    public enum TipoMovimentoEnum
    {
        [Description("C")]
        Credito = 0,
        [Description("D")]
        Debito = 1
    }
}
