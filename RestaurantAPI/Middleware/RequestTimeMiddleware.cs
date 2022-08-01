using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private Stopwatch stopwatch;
        public RequestTimeMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
            stopwatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            stopwatch.Start();
            await next.Invoke(context);
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds / 1000 > 4)
            {
                string path = context.Request.Path;
                string method = context.Request.Method;
                logger.LogInformation($"Request: {method} at {path} took " +
                    $"{stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
