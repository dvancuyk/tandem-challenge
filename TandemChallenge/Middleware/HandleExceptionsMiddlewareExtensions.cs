using Microsoft.AspNetCore.Builder;

namespace TandemChallenge.Api.Middleware
{
    /// <summary>
    /// Extensions for the <see cref="IApplicationBuilder" /> interface.
    /// </summary>
    public static class HandleExceptionsMiddlewareExtensions
    {
        /// <summary>
        /// Add the <see cref="HandleExceptionsMiddleware" /> to the builder.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> being configured.</param>
        /// <param name="rules"></param>
        /// <returns>The <paramref name="builder" /> to enable fluent configuration.</returns>
        /// <remarks>
        /// </remarks>
        public static IApplicationBuilder UseExceptionRules(this IApplicationBuilder builder, params ExceptionRule[] rules)
        {
            builder.UseMiddleware<HandleExceptionsMiddleware>(new HandleExceptionsOptions(rules));
            return builder;
        }
    }
}
