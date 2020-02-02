using System.Threading.Tasks;
using CrossCutting.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace API.Filter
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ErrorHandlingFilter> _Logger;

        public ErrorHandlingFilter(ILogger<ErrorHandlingFilter> logger)
        {
            _Logger = logger;
        }

        public async override Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            await Task.Run(() => _Logger.LogError(exception, string.Empty));

            if (exception is ElementNotFoundException)
            {
                context.Result = new StatusCodeResult(404);
                return;
            }

            if (exception is ValidationException)
            {
                context.Result = new BadRequestObjectResult(exception.Message);
                return;
            }

            context.Result = new BadRequestObjectResult(string.Empty);
        }
    }
}