namespace AuthServer.Models;

public class GrantTokenRequestModel
{
    public string Code { get; set; }

    public GrantTokenRequestModel(string code)
    {
        Code = !string.IsNullOrWhiteSpace(code) ? code : throw new ArgumentNullException(nameof(code));
    }
}