using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Models;

public class SignInRequestModel
{

    [BindProperty]
    public string Username { get; set; }
        
    [BindProperty]
    public string Password { get; set; }

}