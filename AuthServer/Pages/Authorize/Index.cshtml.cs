using AuthServer.UseCases.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthServer.Pages.Authorize
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public AuthenticationRequestModel AuthenticationRequest { get; set; } = new AuthenticationRequestModel();

        public IActionResult OnGet()
        {
            // Validate the incoming request (e.g., check client_id, redirect_uri, etc.)
            if (string.IsNullOrWhiteSpace(AuthenticationRequest.ClientId) ||
                string.IsNullOrWhiteSpace(AuthenticationRequest.RedirectUri))
            {
                return BadRequest("Invalid authorization request.");
            }

            if (!Request.Cookies.TryGetValue("csrfState", out var storedState) || string.IsNullOrEmpty(storedState))
            {
                return BadRequest("Invalid or missing CSRF state.");
            }

            // Check if the user is authenticated
            if (!(bool)HttpContext.User.Identity?.IsAuthenticated && !String.IsNullOrEmpty(Request.QueryString.Value))
            {
                // Preserve the state by appending the current query string to the sign-in redirect
                string currentQueryString = Request.QueryString.Value;
                string returnUrl = $"/oauth2/authorize{currentQueryString}";
                string encodedReturnUrl = Uri.EscapeDataString(returnUrl);
                string signInRedirectUrl = $"/signin?returnUrl={encodedReturnUrl}";
                return Redirect(signInRedirectUrl);
            }


            // Render the page for user interaction
            return Page();
        }
    }
}