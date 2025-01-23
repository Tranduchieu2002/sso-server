namespace AuthServer.Errors;

public static class OAuthErrorCodes
{
    public const string InteractionRequired = "interaction_required";
    public const string LoginRequired = "login_required";
    public const string AccountSelectionRequired = "account_selection_required";
    public const string ConsentRequired = "consent_required";
    public const string InvalidRequestUri = "invalid_request_uri";
    public const string InvalidRequestObject = "invalid_request_object";
    public const string RequestNotSupported = "request_not_supported";
    public const string RequestUriNotSupported = "request_uri_not_supported";
    public const string RegistrationNotSupported = "registration_not_supported";
    public const string InvalidRequest = "invalid_request";
    public const string InvalidScope = "invalid_scope";
}