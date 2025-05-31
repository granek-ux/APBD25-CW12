
using APBD25_CW12.DTO;
using APBD25_CW12.Models;

namespace APBD25_CW12.Services;

public interface ITripsService
{
    public Task<TripsReturnDto> GetTripsAsync(int page, int pageSize,CancellationToken cancellationToken);
    
    public Task AddClientToTripAsync(int idTrip, RequestDto requestDto, CancellationToken cancellationToken);
}