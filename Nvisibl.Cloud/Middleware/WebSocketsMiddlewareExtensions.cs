using Microsoft.AspNetCore.Builder;

namespace Nvisibl.Cloud.Middleware
{
    public static class WebSocketsMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketsMiddleware>();
        }
    }
}
