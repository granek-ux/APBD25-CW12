namespace APBD25_CW12.Services;

public interface IClientService
{
    public Task DeleteClientAsync(int id, CancellationToken cancellationToken);

}