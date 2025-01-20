namespace AuthServer.Models;

public class GrantTokenRequestModel
{
    public GrantTokenRequestModel(string code)
    {
        Code = !string.IsNullOrWhiteSpace(code) ? code : throw new ArgumentNullException(nameof(code));
    }

    public string Code { get; set; }
}