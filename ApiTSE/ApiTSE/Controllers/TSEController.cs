using Microsoft.AspNetCore.Mvc;

namespace ApiTSE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TSEController : ControllerBase
{
    private readonly string securityToken = "789xyz$%&";

    public TSEController()
    {
    }

    [HttpGet("{cedula}")]
    public IActionResult checkCedula(string cedula, [FromHeader(Name = "Auth-Token")] string? token)
    {
        try
        {
            if (token != securityToken)
            {
                return StatusCode(403);
            }
            if (cedula.Length != 9)
            {
                return BadRequest();
            }
            if (cedula[0].Equals('0') || cedula[0].Equals('8'))
            {
                return Ok(new {valid = false});
            }

            var cedulaValida = cedula.All(char.IsDigit) && !cedula.Substring(1,8).All(a => a.Equals('0'));
            return Ok(new { valid = cedulaValida});
        }
        catch {
            return StatusCode(500);
        }
    }
}
