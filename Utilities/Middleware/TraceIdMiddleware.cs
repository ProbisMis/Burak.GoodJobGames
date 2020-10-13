using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace GoodJobGames.Utilities.Middleware
{
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TraceIdMiddlewareOptions _middlewareOptions;

        public TraceIdMiddleware(RequestDelegate next, IOptions<TraceIdMiddlewareOptions> options)
        {
            _next = next;
            _middlewareOptions = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(_middlewareOptions.Header, out StringValues traceId))
            {
                context.TraceIdentifier = traceId;
            }
            else
            {
                string guid = Guid.NewGuid().ToString();
                DateTime requestTime = DateTime.UtcNow;
                context.TraceIdentifier = $"{requestTime:yyyy-MM-dd-HH-mm}:{guid}";
            }

            context.Response.OnStarting(() =>
                                        {
                                            if (_middlewareOptions.IncludeInResponseHeader)
                                            {
                                                context.Response.Headers.Add(_middlewareOptions.Header, context.TraceIdentifier);
                                            }

                                            return Task.CompletedTask;
                                        });

            await _next.Invoke(context);
        }
    }

    public class TraceIdMiddlewareOptions
    {
        public const string DefaultHeader = "x-trace-id";

        public string Header { get; set; } = DefaultHeader;

        public bool IncludeInResponseHeader { get; set; } = true;
    }

    public static class TraceIdMiddlewareExtensions
    {
        public static void UseTraceIdMiddleware(this IApplicationBuilder app, TraceIdMiddlewareOptions correlationIdMiddlewareOptions = null)
        {
            correlationIdMiddlewareOptions = correlationIdMiddlewareOptions ?? new TraceIdMiddlewareOptions();
            app.UseMiddleware<TraceIdMiddleware>(Options.Create(correlationIdMiddlewareOptions));
        }
    }
}