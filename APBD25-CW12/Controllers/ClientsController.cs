using APBD25_CW11.Exceptions;
using APBD25_CW12.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD25_CW12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _clientService.DeleteClientAsync(id, cancellationToken);

            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem("Unexpected error occurred", statusCode: 500);
            }

            return Ok("Client deleted successfully");
        }
        
    }
}
