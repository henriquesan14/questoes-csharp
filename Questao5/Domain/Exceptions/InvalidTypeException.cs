namespace Questao5.Domain.Exceptions
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException(string? message) : base(message)
        {
        }
    }
}
