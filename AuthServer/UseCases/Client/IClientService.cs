namespace AuthServer.UseCases.Client;

public interface IClientService
{
    public Task CreateClientAsync(Client client);
    public Task<Client?> GetClientAsync(string id);
}