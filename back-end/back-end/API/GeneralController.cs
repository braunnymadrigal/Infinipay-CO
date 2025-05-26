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
                var PersonId = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                var Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
                if (Nickname != null && PersonId != null && Role != null)
                {
                    userModel.Nickname = Nickname;
                    userModel.PersonId = PersonId;
                    userModel.Role = Role;
                }
            }
            return userModel;
        }
    }
}
