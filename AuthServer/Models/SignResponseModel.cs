namespace AuthServer.Models;

// Response model for sign-in
public class SignResponseModel
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RedirectUrl { get; set; }
}