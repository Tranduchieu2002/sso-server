namespace AuthServer.Errors;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ApiError errorResponse;

        if (exception is ApiException apiException)
        {
            context.Response.StatusCode = apiException.StatusCode;
            errorResponse = apiException.Error;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            errorResponse = new ApplicationError()
            {
                Error = "server_error",
                ErrorDescription = "An unexpected error occurred.",
                Details = new { exception.Message, exception.StackTrace }
            };
        }

        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}