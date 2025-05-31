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
        public async Task DeleteClientAsync(int id, CancellationToken cancellationToken)
        {
            await _clientService.DeleteClientAsync(id, cancellationToken);
        }
        
    }
}
