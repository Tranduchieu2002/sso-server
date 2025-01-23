namespace AuthServer.Errors;

public static class OAuthErrors
{
    public static OAuthError InteractionRequired(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.InteractionRequired,
            "The Authorization Server requires interaction from the End-User to proceed.", state, details);
    }

    public static OAuthError LoginRequired(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.LoginRequired,
            "The Authorization Server requires End-User authentication.", state, details);
    }

    public static OAuthError AccountSelectionRequired(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.AccountSelectionRequired,
            "The End-User needs to select an account to proceed.", state, details);
    }

    public static OAuthError ConsentRequired(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.ConsentRequired,
            "The Authorization Server requires End-User consent to proceed.", state, details);
    }

    public static OAuthError InvalidRequestUri(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.InvalidRequestUri,
            "The request_uri in the Authorization Request is invalid or returned an error.", state, details);
    }

    public static OAuthError InvalidRequestObject(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.InvalidRequestObject,
            "The request parameter contains an invalid Request Object.", state, details);
    }

    public static OAuthError RequestNotSupported(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.RequestNotSupported,
            "The Authorization Server does not support the use of the request parameter.", state, details);
    }

    public static OAuthError RequestUriNotSupported(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.RequestUriNotSupported,
            "The Authorization Server does not support the use of the request_uri parameter.", state, details);
    }

    public static OAuthError RegistrationNotSupported(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.RegistrationNotSupported,
            "The Authorization Server does not support the use of the registration parameter.", state, details);
    }

    public static OAuthError InvalidRequest(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.InvalidRequest,
            "The request is missing a required parameter, includes an invalid parameter value, or is otherwise malformed.",
            state, details);
    }

    public static OAuthError InvalidScope(string? state = null, object? details = null)
    {
        return OAuthError.Create(OAuthErrorCodes.InvalidScope, "The requested scope is invalid, unknown, or malformed.",
            state, details);
    }
}