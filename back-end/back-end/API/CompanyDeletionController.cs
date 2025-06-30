using back_end.Application;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDeletionController : GeneralController
    {
        private readonly ICompanyDeletion _companyDeletion;

        public CompanyDeletionController()
        {
            _companyDeletion = new CompanyDeletion(
                new CompanyDeletionRepository(new ConnectionRepository(), 
                new UtilityRepository()));
        }

        [Authorize(Roles = "superAdmin")]
        [HttpDelete]
        public IActionResult DeleteCompany(string companyName)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var emails = _companyDeletion.deleteCompany(companyName);
                iActionResult = Ok(emails);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
