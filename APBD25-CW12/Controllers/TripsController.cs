using APBD25_CW11.Exceptions;
using APBD25_CW12.DTO;
using APBD25_CW12.Models;
using APBD25_CW12.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD25_CW12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips(CancellationToken cancellationToken,[FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }
            try
            {
                var trips = await _tripsService.GetTripsAsync(page,pageSize, cancellationToken);
                
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return Problem("Unexpected error occurred", statusCode: 500);
            }
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip ,[FromBody] RequestDto requestDto, CancellationToken cancellationToken)
        {
            try
            {
                await _tripsService.AddClientToTripAsync(idTrip, requestDto, cancellationToken);

            }catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return Problem("Unexpected error occurred", statusCode: 500);
            }

            return Created();
        }
        
    }
}
