using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using SqlVersionService.Application.Abstractions;
using SqlVersionService.Contracts.Requests;
using SqlVersionService.Contracts.Responses;

namespace SqlVersionService.Web.Controllers
{
    [RoutePrefix("api/sql")]
    public sealed class SqlConnectionController : ApiController
    {
        private readonly ISqlConnectionSessionService _service;

        public SqlConnectionController(ISqlConnectionSessionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("connection/open")]
        public async Task<IHttpActionResult> Open(OpenConnectionRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return Content(HttpStatusCode.BadRequest, CreateProblemDetails(
                    status: 400,
                    title: "Bad Request",
                    detail: "Request body is required."));
            }

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, CreateProblemDetails(
                    status: 400,
                    title: "Bad Request",
                    detail: "Request validation failed."));
            }

            var connectionId = await _service.OpenAsync(request.ConnectionString, cancellationToken);

            return Ok(new OpenConnectionResponse
            {
                ConnectionId = connectionId,
                SessionTimeoutSeconds = _service.GetSessionTimeoutSeconds()
            });
        }

        [HttpPost]
        [Route("version")]
        public async Task<IHttpActionResult> GetVersion(GetVersionRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.ConnectionId == Guid.Empty)
            {
                return Content(HttpStatusCode.BadRequest, CreateProblemDetails(
                    status: 400,
                    title: "Bad Request",
                    detail: "ConnectionId is required."));
            }

            var version = await _service.GetVersionAsync(request.ConnectionId, cancellationToken);

            return Ok(new GetVersionResponse
            {
                Version = version
            });
        }

        [HttpPost]
        [Route("connection/close")]
        public async Task<IHttpActionResult> Close(CloseConnectionRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.ConnectionId == Guid.Empty)
            {
                return Content(HttpStatusCode.BadRequest, CreateProblemDetails(
                    status: 400,
                    title: "Bad Request",
                    detail: "ConnectionId is required."));
            }

            var closed = await _service.CloseAsync(request.ConnectionId, cancellationToken);
            if (!closed)
            {
                return Content(HttpStatusCode.NotFound, CreateProblemDetails(
                    status: 404,
                    title: "Not found",
                    detail: $"Connection {request.ConnectionId} not found."));
            }

            return Ok(new CloseConnectionResponse
            {
                Closed = closed
            });
        }

        private ProblemDetailsResponse CreateProblemDetails(int status, string title, string detail)
        {
            return new ProblemDetailsResponse
            {
                Type = $"https://httpstatuses.com/{status}",
                Title = title,
                Status = status,
                Detail = detail,
                Instance = Request?.RequestUri?.AbsolutePath
            };
        }
    }
}