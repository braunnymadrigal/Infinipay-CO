//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using back_end.Models;
//using back_end.Repositories;
//using System.Security.Cryptography.X509Certificates;

//namespace back_end.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BeneficioController : ControllerBase
//    {
//        private readonly BeneficioRepository _beneficioRepository;
//        public BeneficioController()
//        {
//           _beneficioRepository = new BeneficioRepository();
//        }
//        [HttpGet]
//        public List<BeneficioModel> Get()
//        {
//            var beneficios = _beneficioRepository.ObtenerBeneficios();
//            return beneficios;
//        }
//        [HttpPost]
//        public async Task<ActionResult<bool>> CrearBeneficio(BeneficioModel beneficio)
//        {
//            try
//            {
//                if (beneficio == null)
//                {
//                    return BadRequest();
//                }

//                BeneficioRepository beneficioRepository = new BeneficioRepository();
//                var result = beneficioRepository.CrearBeneficio(beneficio);
//                return new JsonResult(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, "Error creando un beneficio");
//            }
//        }

//    }
//}
