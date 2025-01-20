using System.Security.Claims;
using AuthServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthServer.UseCases.Login;

public class SignInManager(IAuthenticationService authenticationService) : ISignInManager
{
    // Mocked user data for demonstration. Replace this with database lookup or external authentication.
    private readonly Dictionary<string, string> _users = new()
    {
        { "admin", "password123" }, // username: admin, password: password123
        { "user", "mypassword" } // username: user, password: mypassword
    };


    // Sign-in handler
    public async Task<SignResponseModel> Handle(HttpContext context, SignInRequestModel signInRequestModel)
    {
        // Validate inputs
        if (string.IsNullOrEmpty(signInRequestModel.Username) || string.IsNullOrEmpty(signInRequestModel.Password))
        {
            return new SignResponseModel
            {
                IsSuccess = false,
                ErrorMessage = "Both fields are required."
            };
        }

        // Authenticate user
        if (_users.TryGetValue(signInRequestModel.Username, out var storedPassword) &&
            storedPassword == signInRequestModel.Password)
        {
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInRequestModel.Username),
                new Claim(ClaimTypes.Role, "User") // Assign roles dynamically as needed
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user
            await authenticationService.SignInAsync(
                context,
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true, // Use RememberMe flag from request
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30) // Set expiration time
                });

            // Return success response
            return new SignResponseModel
            {
                IsSuccess = true,
                RedirectUrl = "/" // Redirect to a secure page after successful login
            };
        }

        // Authentication failed
        return new SignResponseModel
        {
            IsSuccess = false,
            ErrorMessage = "Invalid username or password."
        };
    }
}