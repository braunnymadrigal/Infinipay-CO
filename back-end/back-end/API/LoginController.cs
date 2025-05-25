using back_end.Infraestructure;
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
        private readonly LoginRepository loginRepository;

        public LoginController(IConfiguration config)
        {
            this.config = config;
            loginRepository = new LoginRepository(this.config);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUserModel loginUserModel)
        {
            IActionResult iActionResult = BadRequest("UNKNOWN ERROR");
            try
            {
                var token = loginRepository.Login(loginUserModel);
                iActionResult = Ok(token);
            } catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
