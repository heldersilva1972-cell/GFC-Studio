// [NEW]
using GFC.BlazorServer.Services.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ISystemPerformanceService performanceService)
        {
            performanceService.IncrementRequestCount();
            await _next(context);
        }
    }
}
