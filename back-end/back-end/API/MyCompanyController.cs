using back_end.Repositories;
using back_end.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyCompanyController : ControllerBase
    {
        private readonly MyCompanyRepository _myCompanyRepository;

        public MyCompanyController()
        {
            _myCompanyRepository = new MyCompanyRepository();
        }

        [Authorize(Roles = "empleador")]
        [HttpGet]
        public MyCompanyModel Get()
        {
            string ownerId = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var sid = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                if (sid != null)
                {
                    ownerId = sid;
                }
            }
            MyCompanyModel myCompanyModel = _myCompanyRepository.Get(ownerId);
            return myCompanyModel;
        }
    }
}
