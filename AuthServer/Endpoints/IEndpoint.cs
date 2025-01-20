namespace AuthServer.Enpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}