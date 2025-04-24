namespace WebSocketChat.Services
{
    public class ChatHubMiddleware
    {
        readonly RequestDelegate next;
        public ChatHubMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext ctx, ChatService chatService)
        {
            if (!ctx.WebSockets.IsWebSocketRequest)
            {
                await next.Invoke(ctx);
            }
            else if (ctx.User.Identity?.IsAuthenticated ?? false)
            {
                await chatService.NewClientTask(await ctx.WebSockets.AcceptWebSocketAsync(), ctx.User.Identity.Name!);
            }
            else
            {
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
