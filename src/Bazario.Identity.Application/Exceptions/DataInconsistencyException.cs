namespace Bazario.Identity.Application.Exceptions
{
    public sealed class DataInconsistencyException : Exception
    {
        public DataInconsistencyException()
            : base("Data inconsistency detected.")
        { }
    }
}
