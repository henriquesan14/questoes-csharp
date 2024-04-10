using System;

namespace Questao1
{
    public class ContaBancaria {
        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            DepositoInicial = depositoInicial;
            Saldo = depositoInicial;
        }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0;
        }

        public int Numero { get; private set; }
        public string Titular { get; private set; }
        public double DepositoInicial { get; private set; }

        public double Saldo { get; private set; }

        public void SetTitular(string titular)
        {
            Titular = titular;
        }


        public void Deposito(double quantia)
        {
            if(quantia > 0)
            {
                Saldo += quantia;
            }
            Console.WriteLine("Quantia não pode ser menor ou igual a zero.");
        }

        public void Saque(double quantia)
        {
            if (quantia <= 0)
            {
                Console.WriteLine("Quantia não pode ser menor ou igual a zero.");
                return;
            }
            if (quantia <= Saldo)
            {
                Saldo -= (quantia + 3.5);
            }
            Console.WriteLine("Saldo insuficiente.");
        }

        public override string ToString()
        {
            return $"Conta: {Numero}, Titular: {Titular}, Saldo: {Saldo:C2}";
        }

    }
}
