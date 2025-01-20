using System.ComponentModel.DataAnnotations;

namespace AuthServer.Helpers;

public class OidcOptions
{
    [Required(ErrorMessage = "Authority is required.")]
    [Url(ErrorMessage = "Authority must be a valid URL.")]
    public string Authority { get; set; }

    [Required(ErrorMessage = "ClientId is required.")]
    public string ClientId { get; set; }

    [Required(ErrorMessage = "ClientSecret is required.")]
    public string ClientSecret { get; set; }

    [Required(ErrorMessage = "RedirectUri is required.")]
    [Url(ErrorMessage = "RedirectUri must be a valid URL.")]
    public string RedirectUri { get; set; }

    [Required(ErrorMessage = "PostLogoutRedirectUri is required.")]
    [Url(ErrorMessage = "PostLogoutRedirectUri must be a valid URL.")]
    public string PostLogoutRedirectUri { get; set; }

    [Required(ErrorMessage = "Scope is required.")]
    public string Scope { get; set; }

    public bool RequireHttps { get; set; }
}