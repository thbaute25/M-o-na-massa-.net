using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "OK",
            message = "API Mão na Massa está funcionando!",
            timestamp = DateTime.UtcNow
        });
    }
}

