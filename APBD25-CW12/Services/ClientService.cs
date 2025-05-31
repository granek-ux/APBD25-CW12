using APBD25_CW11.Exceptions;
using APBD25_CW12.Data;
using Microsoft.EntityFrameworkCore;

namespace APBD25_CW12.Services;

public class ClientService : IClientService
{
    private readonly ApbdContext _context;

    public ClientService(ApbdContext context)
    {
        _context = context;
    }

    public async Task DeleteClientAsync(int id, CancellationToken cancellationToken)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == id, cancellationToken);

        if (client == null)
        {
            throw new NotFoundException($"Client not found {id}");
        }

        if (client.ClientTrips.Count != 0)
        {
            throw new BadRequestException("Client has trips assigned and cannot be deleted");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(cancellationToken);
    }
}