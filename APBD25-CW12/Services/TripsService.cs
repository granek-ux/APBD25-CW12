using APBD25_CW11.Exceptions;
using APBD25_CW12.Data;
using APBD25_CW12.DTO;
using APBD25_CW12.Models;
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

    public async Task AddClientToTripAsync(int idTrip, RequestDto requestDto, CancellationToken cancellationToken)
    {
        
        var client = await _context.Clients.Where(c => c.Pesel == requestDto.Pesel)
            .FirstOrDefaultAsync(cancellationToken);
            if (client != null)
            {
                throw new ConflictException($"Client with PESEL {requestDto.Pesel} was found.");
            }
            
            
            var trip = await _context.Trips.Where( t => t.IdTrip == idTrip)
                .Include(t => t.ClientTrips)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (trip == null)
            {
                throw new NotFoundException($"Trip with ID {idTrip} not found.");
            }
            
            if (trip.DateFrom < DateTime.Now)
            {
                throw new BadRequestException("Cannot add client to a trip that has already started.");
            }
            
            var newClient = new Client
            {
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Email = requestDto.Email,
                Telephone = requestDto.Telephone,
                Pesel = requestDto.Pesel
            };
            
            var isAlreadyOnTrip = await _context.ClientTrips
                .Include(e => e.IdClientNavigation)
                .AnyAsync(ct => ct.IdClientNavigation.Pesel == requestDto.Pesel && ct.IdTrip == idTrip, cancellationToken);
            
            if (isAlreadyOnTrip)
            {
                throw new ConflictException($"Client with PESEL {requestDto.Pesel} is already on this trip.");
            }
            
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await _context.Clients.AddAsync(newClient, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                var clientTrip = new ClientTrip
                {
                    IdClient = newClient.IdClient,
                    IdTrip = idTrip,
                    RegisteredAt = DateTime.Now,
                    PaymentDate = requestDto.PaymentDate
                };
                await _context.ClientTrips.AddAsync(clientTrip, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("An error occurred while adding the client to the trip.");
            }
    }
}