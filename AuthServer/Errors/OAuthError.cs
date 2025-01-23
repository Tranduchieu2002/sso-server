namespace AuthServer.Errors;

public class OAuthError : ApiError
{
    public static OAuthError Create(string error, string description, string? state = null, object? details = null,
        string? errorUri = null)
    {
        return new OAuthError
        {
            Error = error,
            ErrorDescription = description,
            State = state,
            Details = details,
            ErrorUri = errorUri
        };
    }
}