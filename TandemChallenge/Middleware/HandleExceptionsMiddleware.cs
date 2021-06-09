using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TandemChallenge.Api.Middleware
{
    /// <summary>
    /// Middleware for logging unhandled errors in the app pipeline.
    /// </summary>
    public class HandleExceptionsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<HandleExceptionsMiddleware> logger;
        private readonly ExceptionRule[] rules;

        /// <summary>
        /// Construct a LogUnhandledExceptionsMiddleware
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate" /> that is next in the call chain.</param>
        /// <param name="options">Configures how this middleware should work.</param>
        /// <param name="logger"></param>
        public HandleExceptionsMiddleware(RequestDelegate next, HandleExceptionsOptions options, ILogger<HandleExceptionsMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
            rules = options.Rules;
        }

        /// <summary>
        /// The method invoked when a request is processed.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext" /> for the request.</param>
        /// <returns>A <see cref="Task" /> that can be awaited.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                var type = exception.GetType();
                var rule = rules.FirstOrDefault(r => r.ExceptionType == type);

                if (rule == null)
                {
                    // If we don't have a rule and have arrived here, set status as 500 and log the exception
                    logger.LogDebug($"No rule currently exists for the exception type {type.Name}.");
                    SetServerErrorCodeAndLogException(context, exception);
                }
                else
                {
                    // Process rules
                    SetStatusCode(context, (int)rule.ExpectedReturnStatus, exception.Message);
                }

                var result = JsonConvert.SerializeObject(new { message = exception.Message });

                await context.Response.WriteAsync(result);
            }
        }

        private void ResetResponse(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
        }

        private void SetStatusCode(HttpContext context, int statusCode, string message)
        {
            ResetResponse(context);
            context.Response.StatusCode = statusCode;
        }

        private void SetServerErrorCodeAndLogException(HttpContext context, Exception exception)
        {
            ResetResponse(context);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
