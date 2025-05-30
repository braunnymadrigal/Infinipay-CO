using back_end.Application;
using back_end.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : GeneralController
    {
        private readonly IConfiguration config;
        private readonly ILogin login;

        public LoginController(IConfiguration config)
        {
            this.config = config;
            login = new Login(this.config);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUserModel loginUserModel)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var token = login.LogUser(loginUserModel);
                iActionResult = Ok(token);
            } catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
