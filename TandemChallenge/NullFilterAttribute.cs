using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TandemChallenge
{
    /// <summary>
    /// Filters null responses to return a <see cref="NotFoundResult"/>
    /// </summary>
    public class NullFilterAttribute : Attribute, IActionFilter
    {
        /// <inheritdoc />
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult result && result.Value == null)
            {
                context.Result = new NotFoundResult();
            }
        }

        /// <inheritdoc />
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // no operation
        }
    }
}
