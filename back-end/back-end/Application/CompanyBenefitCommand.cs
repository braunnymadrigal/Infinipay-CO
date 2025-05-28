using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class CompanyBenefitCommand : ICompanyBenefitCommand
  {
    private readonly CompanyBenefitRepository companyBenefitRepository;

    public CompanyBenefitCommand(CompanyBenefitRepository companyBenefitRepository)
    {
      this.companyBenefitRepository = companyBenefitRepository;
    }

    public void CreateBenefit(CompanyBenefitDTO benefit
      , string loggedUserNickname)
    {
      if (string.IsNullOrWhiteSpace(loggedUserNickname))
      {
        throw new ArgumentException("loggedUserNickname is required");
      }

      if (benefit == null)
      {
        throw new ArgumentNullException(nameof(benefit));
      }
      try 
      {
        companyBenefitRepository.CreateBenefit(benefit, loggedUserNickname);
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("BENEFICIO_DUPLICADO"))
                    throw new Exception("Ya existe un beneficio registrado con ese nombre.");
        throw new Exception("Error inesperado creando beneficio." +
                    " Detalles: " + ex.Message);
      }
    }

  }
}