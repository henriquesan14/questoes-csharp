using Questao5.Domain.Exceptions;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Movimento(string contaCorrenteId, string tipoMovimento, decimal valor)
        {
            Id = Guid.NewGuid().ToString();
            ContaCorrenteId = contaCorrenteId;
            DataMovimento = DateTime.Now;
            TipoMovimento = tipoMovimento;
            Valor = valor;
            this.Validate();
        }

        public Movimento()
        {
            this.Validate();
        }

        public string Id { get; set; }
        public string ContaCorrenteId { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public string TipoMovimento { get; private set; }
        public decimal Valor { get; private set; }

        private void Validate()
        {
            if (Valor < 0)
            {
                throw new InvalidValueException("Apenas valores positivos podem ser recebidos");
            }
            if(TipoMovimento != "C" && TipoMovimento != "D")
            {
                throw new InvalidTypeException("Apenas os tipos “débito” ou “crédito” podem ser aceitos");
            }
        }

    }
}
