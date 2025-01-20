namespace AuthServer.UseCases.Client
{
    public interface IClientRepository
    {
        Task<Client?> GetClientById(string id);
        Task SaveClient(string id, Client client);
    }
}