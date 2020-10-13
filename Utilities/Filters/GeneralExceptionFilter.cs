using System;
using System.Net;
using GoodJobGames.Models.CustomExceptions;
using GoodJobGames.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GoodJobGames.Utilities.Filters
{
    public class GeneralExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GeneralExceptionFilter> _logger;

        public GeneralExceptionFilter(ILogger<GeneralExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var basicErrorResponse = new BasicErrorResponse();

            string traceId = context.HttpContext.TraceIdentifier;

            Exception ex = context.Exception;
            HttpStatusCode httpStatusCode;
            basicErrorResponse.Message = context.Exception.Message;

            if (ex is NotFoundException)
            {
                _logger.LogWarning(ex, $"TraceId: {traceId} - {basicErrorResponse.Message}", null);
                httpStatusCode = HttpStatusCode.NotFound;
            }
            else if (ex is ValidationException)
            {
                _logger.LogWarning(ex, $"TraceId: {traceId} - {basicErrorResponse.Message}", null);
                httpStatusCode = HttpStatusCode.BadRequest;
            }
            else if (ex is ConflictException)
            {
                _logger.LogWarning(ex, $"TraceId: {traceId} - {basicErrorResponse.Message}", null);
                httpStatusCode = HttpStatusCode.Conflict;
            }
            else if (ex is PermissionException)
            {
                _logger.LogWarning(ex, $"TraceId: {traceId} - {basicErrorResponse.Message}", null);
                httpStatusCode = HttpStatusCode.Forbidden;
            }
            else if (ex is IntegrationException)
            {
                _logger.LogError(ex, $"TraceId: {traceId} - {basicErrorResponse.Message}", null);
                httpStatusCode = HttpStatusCode.BadGateway;
            }
            else if (ex is AuthenticationException)
            {
                _logger.LogError(ex, $"TraceId: {traceId} - {basicErrorResponse.Message}", null);
                httpStatusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                _logger.LogError(ex, $"TraceId: {traceId} - {basicErrorResponse.Message}", null);
                httpStatusCode = HttpStatusCode.InternalServerError;
            }

            context.Result = new ObjectResult(basicErrorResponse)
                             {
                                 StatusCode = (int) httpStatusCode
                             };
        }
    }
}