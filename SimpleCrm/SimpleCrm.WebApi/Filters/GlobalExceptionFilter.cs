using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void Dispose() { }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            var item = exception is ApiException
                ? ((ApiException)exception).Model
                : context.RouteData.Values;
            var statusCode = exception is ApiException
                ? ((ApiException)exception).StatusCode
                : 200; //most endpoint consumers prefer the success model at this time
            var message = exception is ApiException
                ? ((ApiException)exception).Message
                : "An error occured";

            var model = new
            {
                success = false,
                messages = new string[] { message },
                item = item
            };

            var eventId = new EventId(statusCode);
            _logger.LogError(eventId, exception, "An error occured, was model state valid: {0}, Exception details: {1}", context.ModelState.IsValid, exception.StackTrace);
            context.Result = new ObjectResult(model) { StatusCode = statusCode };
        }
    }
}
