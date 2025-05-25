using back_end.Domain;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace back_end.API
{
    public abstract class GeneralController : ControllerBase
    {
        protected UserModel GetUser()
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
