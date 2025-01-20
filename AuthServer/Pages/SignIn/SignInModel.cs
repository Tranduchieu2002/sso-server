using AuthServer.Models;
using AuthServer.UseCases.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthServer.Pages.SignIn
{
    public class SignInModel : PageModel
    {
        private readonly ISignInManager _signInManager;

        public SignInModel(ISignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty] public SignInRequestModel LoginDetails { get; set; } = new SignInRequestModel();

        [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = "/";

        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return Redirect(ReturnUrl);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid input. Please try again.");
                return Page();
            }

            var result = await _signInManager.Handle(HttpContext, LoginDetails);

            if (result.IsSuccess)
            {
                return Redirect(ReturnUrl);
            }

            // Display the specific error from the result
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Invalid login attempt.");
            return Page();
        }
    }
}