using back_end.Application;
using Microsoft.AspNetCore.Mvc;
using back_end.Models;
using Microsoft.IdentityModel.Tokens;
using back_end.API;

namespace back_end.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class EmailController : GeneralController
  {
    private readonly IEmailCommand emailService;

    public EmailController(IEmailCommand emailService)
    {
      this.emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> sendEmail([FromForm] EmailModel email)
    {
      try
      {
        var loggedUserId = string.Empty;

        if (email.recipients.IsNullOrEmpty())
        {
          loggedUserId = GetUser().PersonId;
        }

        var errorMsg = await emailService.sendEmail(email, loggedUserId);
        return Ok(errorMsg);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
