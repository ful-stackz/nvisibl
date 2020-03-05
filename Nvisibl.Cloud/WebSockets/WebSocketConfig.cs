using System;

namespace Nvisibl.Cloud.WebSockets
{
    internal static class WebSocketConfig
    {
        internal static string AllowedOrigins { get; } = "*";
        internal static TimeSpan KeepAliveInterval { get; } = TimeSpan.FromMinutes(4);
        internal static int ReceiveBufferSize { get; } = 1024 * 4;
    }
}
