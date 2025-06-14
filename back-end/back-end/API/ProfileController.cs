﻿using back_end.Infraestructure;
using back_end.Domain;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace back_end.API
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

        [Authorize(Roles = "empleador, empleado, supervisor, administrador, sinRol")]
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
