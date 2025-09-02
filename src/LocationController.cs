using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _service = new LocationService();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetLocationAsync();
            return Ok(result);
        }
    }
}
