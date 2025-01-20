namespace AuthServer.UseCases.Client;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        clientRepository.SaveClient(
            "1234",
            new Client(id: "1234", name: "Hieu", email: "hieu@gmail.com", phone: "1234",
                redirectUris: ["http://localhost:5040"]));
        _clientRepository = clientRepository;
    }

    public async Task CreateClientAsync(Client input)
    {
        // Attempt to retrieve client from cache
        var client = await _clientRepository.GetClientById(input.Id);

        if (client != null) await _clientRepository.SaveClient(input.Id, client);
    }

    public async Task<Client?> GetClientAsync(string id)
    {
        return await _clientRepository.GetClientById(id);
    }
}