namespace Questao5.Domain.Exceptions
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException(string? message) : base(message)
        {
        }
    }
}
