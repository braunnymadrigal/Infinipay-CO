using back_end.Repositories;
using back_end.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileRepository _profileRepository;

        public ProfileController()
        {
            _profileRepository = new ProfileRepository();
        }

        [Authorize]
        [HttpGet]
        public ProfileModel Get()
        {
            string tablaPersonaId = "";
            ProfileModel profileModel = new ProfileModel();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var nameIdentifier = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                var sid = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                var role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
                if (nameIdentifier != null && sid != null && role != null)
                {
                    profileModel.NombreUsuario = nameIdentifier;
                    tablaPersonaId = sid;
                    profileModel.Rol = role;
                }
            }
            profileModel = _profileRepository.GetProfileModel(profileModel, tablaPersonaId);
            return profileModel;
        }
    }
}
