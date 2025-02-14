namespace AuthServer.Errors;

public abstract class ApiException(int statusCode, string error, string description, object? details = null)
    : Exception
{
    public int StatusCode { get; } = statusCode;

    public ApiError Error { get; } = new ApplicationError()
    {
        Error = error,
        ErrorDescription = description,
        Details = details
    };
}