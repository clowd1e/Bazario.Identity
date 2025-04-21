using Bazario.Identity.WebAPI.Middleware;

namespace Bazario.Identity.WebAPI.Extensions.DI
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder AddMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlingMiddleware>();

            return builder;
        }
    }
}
