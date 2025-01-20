namespace AuthServer.Middlewares;

public class SessionMiddleware(RequestDelegate next): IMiddleware
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
    }

}