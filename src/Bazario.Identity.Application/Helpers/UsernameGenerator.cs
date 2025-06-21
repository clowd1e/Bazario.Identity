using System.Text;

namespace Bazario.Identity.Application.Helpers
{
    public static class UsernameGenerator
    {
        public static string Generate(string userId)
        {
            var sb = new StringBuilder();

            sb.Append("user");
            sb.Append('_');
            sb.Append(userId);

            return sb.ToString();
        }
    }
}
