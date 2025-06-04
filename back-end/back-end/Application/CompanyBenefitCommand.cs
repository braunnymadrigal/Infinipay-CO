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
        if (ex.Message.Contains("EMPRESA_NO_ENCONTRADA"))
                    throw new Exception("No se encontró la empresa asociada.");
        throw new Exception("Error inesperado creando beneficio." +
                    " Detalles: " + ex.Message);
      }
    }

    public void UpdateBenefit(Guid id, CompanyBenefitDTO benefit
      , string loggedUserNickname)
    {
      if (string.IsNullOrWhiteSpace(loggedUserNickname))
      {
        throw new ArgumentException("loggedUserNickname is required");
      }

      if (benefit == null || id == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(benefit));
      }
      try 
      {
        companyBenefitRepository.UpdateBenefit(id, benefit, loggedUserNickname);
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("BENEFICIO_DUPLICADO"))
          throw new Exception("Ya existe un beneficio registrado con ese nombre.");
        if (ex.Message.Contains("BENEFICIO_NO_ENCONTRADO"))
          throw new Exception("Beneficio no encontrado.");
        if (ex.Message.Contains("BENEFICIO_NO_PUEDE_SER_MODIFICADO"))
          throw new Exception("El beneficio no puede ser modificado.");
        if (ex.Message.Contains("EMPRESA_NO_ENCONTRADA"))
          throw new Exception("No se encontró la empresa asociada.");
        throw new Exception("Error inesperado actualizando beneficio." +
          " Detalles: " + ex.Message);
      }
    }
  }
}