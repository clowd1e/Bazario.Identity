namespace Bazario.Identity.Application.Exceptions
{
    public sealed class NotValidatedDataException : Exception
    {
        public NotValidatedDataException(string message)
            : base(message)
        { }
    }
}
