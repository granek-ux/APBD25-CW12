using APBD25_CW12.Data;
using APBD25_CW12.DTO;
using Microsoft.EntityFrameworkCore;

namespace APBD25_CW12.Services;

public class TripsService : ITripsService
{
    private readonly ApbdContext _context;

    public TripsService(ApbdContext context)
    {
        _context = context;
    }


    public async Task<TripsReturnDto> GetTripsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips.Include(t => t.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries).Select( e => new TripsDto()
            {
                Name = e.Name,
                DateFrom = e.DateFrom,
                DateTo = e.DateTo,
                Description = e.Description,
                MaxPeople = e.MaxPeople,
                Countries = e.IdCountries.Select(c => new CountryDto
                {
                    Name = c.Name,
                }).ToList(),
                Clients = e.ClientTrips.Select(ct => new ClientDto
                {
                    FisrtName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);


        return new TripsReturnDto()
        {
            trips = trip,
            allPages = await _context.Trips.CountAsync(cancellationToken) / pageSize,
            pageNum = page,
            pageSize = pageSize
        };
    }
}