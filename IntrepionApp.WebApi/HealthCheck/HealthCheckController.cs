using Microsoft.AspNetCore.Mvc;

namespace IntrepionApp.WebApi.HealthCheck;

public class HealthCheckController : ControllerBase
{
    public IActionResult Get()
    {
        return Ok("");
    }
}
