using back_end.Repositories;
using back_end.Models;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly LoginRepository loginRepository;

        public LoginController(IConfiguration config)
        {
            _config = config;
            loginRepository = new LoginRepository(_config);
        }

        [AllowAnonymous]
        [Route("userLogin")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUserModel loginUserModel)
        {
            IActionResult returnActionResult = NotFound("CUSTOM ERROR:user not found");
            UserModel userModel = loginRepository.Authenticate(loginUserModel);
            if (userModel.Nickname != "")
            {
                String token = loginRepository.Generate(userModel);
                if (token != "")
                {
                    returnActionResult = Ok(token);
                }
            }
            return returnActionResult;
        }

        [Route("GetUser")]
        [HttpPost]
        public UserModel GetCurrentUser()
        {
            UserModel userModel = new UserModel();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                var Nickname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                var PersonaId = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                var Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;

                if (Nickname != null && PersonaId != null && Role != null)
                {
                    userModel.Nickname = Nickname;
                    userModel.PersonaId = PersonaId;
                    userModel.Role = Role;
                }
            }
            return userModel;
        }
    }
}
