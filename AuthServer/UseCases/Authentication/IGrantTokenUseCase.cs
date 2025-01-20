using AuthServer.Models;

namespace AuthServer.UseCases.Authentication;

public interface IGrantTokenUseCase
{
    public Task<TokenResponse> Handle(string code);

}