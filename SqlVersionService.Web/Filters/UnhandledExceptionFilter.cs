using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Microsoft.Extensions.Logging;
using SqlVersionService.Contracts.Responses;

namespace SqlVersionService.Web.Filters
{
    public sealed class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<UnhandledExceptionFilter> _logger;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");

            var statusCode = HttpStatusCode.InternalServerError;
            var problem = new ProblemDetailsResponse
            {
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred.",
                Instance = context.Request?.RequestUri?.AbsolutePath
            };

            if (context.Exception is InvalidOperationException ex)
            {
                statusCode = HttpStatusCode.BadRequest;
                problem = new ProblemDetailsResponse
                {
                    Type = "https://httpstatuses.com/400",
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = ex.Message,
                    Instance = context.Request?.RequestUri?.AbsolutePath
                };
            }
            else if (context.Exception is ArgumentException ex2)
            {
                statusCode = HttpStatusCode.BadRequest;
                problem = new ProblemDetailsResponse
                {
                    Type = "https://httpstatuses.com/400",
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = ex2.Message,
                    Instance = context.Request?.RequestUri?.AbsolutePath
                };
            }

            context.Response = context.Request.CreateResponse(statusCode, problem);
        }
    }
}