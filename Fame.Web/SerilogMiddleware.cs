using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Fame.Web
{
    internal class SerilogMiddleware
    {
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private List<string> ignoredPaths = new List<string>() { "/Home/Error", "/hangfire/stats", "/api/Version" };

        private static readonly ILogger Log = Serilog.Log.ForContext<SerilogMiddleware>();

        public readonly RequestDelegate Next;

        public SerilogMiddleware(RequestDelegate next)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var start = Stopwatch.GetTimestamp();
            try
            {
                await Next(httpContext);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log;
                if (!ignoredPaths.Contains(httpContext.Request.Path)) log.Write(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, elapsedMs);
            }
            // Never caught, because `LogException()` returns false.
            catch (Exception ex) when (LogException(httpContext, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex)) { }
        }

        private static bool LogException(HttpContext httpContext, double elapsedMs, Exception ex)
        {
            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, elapsedMs);

            return false;
        }

        private static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.Query.Count > 0)
            {
                result = result.ForContext("RequestQueryString", request.Query.ToDictionary(v => v, v => v.Value.ToString()));
            }

            return result;
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}
