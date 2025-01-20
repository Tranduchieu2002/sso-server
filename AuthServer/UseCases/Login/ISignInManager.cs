using AuthServer.Models;

namespace AuthServer.UseCases.Login;

public interface ISignInManager
{
    public Task<SignResponseModel> Handle(HttpContext request, SignInRequestModel model);
}