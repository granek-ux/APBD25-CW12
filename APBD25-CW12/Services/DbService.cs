using APBD25_CW12.Data;
using Microsoft.EntityFrameworkCore;

namespace APBD25_CW12.Services;

public class DbService : IDbService
{
    private readonly ApbdContext _context;

    public DbService(ApbdContext context)
    {
        _context = context;
    }

    
}