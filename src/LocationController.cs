using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _service;

        // MODIFIQUE O CONTROLLER PARA RECEBER O SERVIÇO VIA CONSTRUTOR
        public LocationController(LocationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetLocationAsync();
            return Ok(result);
        }
    }
}