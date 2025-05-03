namespace Bazario.Identity.Application.Abstractions.Security
{
    public interface IHasher
    {
        string Hash(string value);
    }
}
