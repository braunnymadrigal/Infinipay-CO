using back_end.Repositories;
using back_end.Models;

using Microsoft.AspNetCore.Mvc;

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
    }
}
