namespace Questao5.Domain.Exceptions
{
    public class InvalidAccountException : Exception
    {
        public InvalidAccountException(string? message) : base(message)
        {
        }
    }
}
