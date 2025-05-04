using back_end.Repositories;
using back_end.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly LoginRepository _loginRepository;

        public LoginController(IConfiguration config)
        {
            _config = config;
            _loginRepository = new LoginRepository(_config);
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUserModel loginUserModel)
        {
            IActionResult returnActionResult = NotFound("CUSTOM ERROR:user not found");
            UserModel userModel = _loginRepository.Authenticate(loginUserModel);
            if (userModel.Nickname != "")
            {
                String token = _loginRepository.Generate(userModel);
                if (token != "")
                {
                    returnActionResult = Ok(token);
                }
            }
            return returnActionResult;
        }

        [AllowAnonymous]
        [Route("GetLoggedUser")]
        [HttpGet]
        public UserModel GetLoggedUser()
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
