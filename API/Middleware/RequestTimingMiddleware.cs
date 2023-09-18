using System.Diagnostics;
using Serilog;
namespace Internal.API.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception and its stack trace for unhandled exceptions during request processing
            Log.Error(ex, "Unhandled exception during request processing");
            throw; // Rethrow the exception to allow the global error handling middleware to catch it
        }
        finally
        {
            stopwatch.Stop();

            var request = context.Request;
            var response = context.Response;

            // Log the request method, path, status code, and execution time in milliseconds

            Log.Information("===============================================================");
            Log.Information("HTTP {Method} {Path} responded with {StatusCode} in {Elapsed} ms",
                request.Method,
                request.Path,
                response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
            Log.Information("===============================================================");

            // If the response status code represents an error (4xx or 5xx), log it as a failure
            if (response.StatusCode >= 400 && response.StatusCode <= 599)
            {
                Log.Information("===============================================================");
                Log.Warning("HTTP {Method} {Path} failed with status code {StatusCode}",
                    request.Method,
                    request.Path,
                    response.StatusCode
                );
                Log.Information("===============================================================");
            }
        }
    }
}

