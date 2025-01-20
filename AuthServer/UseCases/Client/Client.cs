namespace AuthServer.UseCases.Client;

public class Client
{
    public Client()
    {
    }

    public Client(string id, string name, string email, string phone, string[] redirectUris)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        RedirectUris = redirectUris;
    }

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string[] RedirectUris { get; set; } = [];
}