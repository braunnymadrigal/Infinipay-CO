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
        if (!IsDeductionTypeValid(benefit))
          throw new Exception("Tipo de deducción inválido.");

        if (!IsElegibleEmployeesValid(benefit))
            throw new Exception("Tipo de empleado elegible inválido.");

        if (!IsMinEmployeeTimeValid(benefit))
            throw new Exception("Tiempo mínimo de empleado inválido.");

        if (benefit.benefit.deductionType == "porcentaje" && !IsPercentageValid(benefit))
            throw new Exception("Porcentaje inválido.");
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
        if (!IsDeductionTypeValid(benefit))
          throw new Exception("Tipo de deducción inválido.");

        if (!IsElegibleEmployeesValid(benefit))
            throw new Exception("Tipo de empleado elegible inválido.");

        if (!IsMinEmployeeTimeValid(benefit))
            throw new Exception("Tiempo mínimo de empleado inválido.");

        if (benefit.benefit.deductionType == "porcentaje" && !IsPercentageValid(benefit))
            throw new Exception("Porcentaje inválido.");
        
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

    public bool IsElegibleEmployeesValid(CompanyBenefitDTO benefit)
    {
      if (string.IsNullOrWhiteSpace(benefit.benefit.elegibleEmployees))
      {
        return false;
      }
      var employees = benefit.benefit.elegibleEmployees;
      var opcionesValidas = new List<string>
      {
        "tiempoCompleto",
        "medioTiempo",
        "horas",
        "servicios",
        "todos"
      };
      return opcionesValidas.Contains(employees);
    }

    public bool IsMinEmployeeTimeValid(CompanyBenefitDTO benefit)
    {
      if (benefit.benefit.minEmployeeTime < 0)
      {
        return false;
      }
      return true;
    }

    public bool IsPercentageValid(CompanyBenefitDTO benefit)
    {
      if (benefit.benefit.deductionType == "porcentaje" &&
          string.IsNullOrWhiteSpace(benefit.benefit.paramOneAPI))
      {
          return false;
      }

      if (!int.TryParse(benefit.benefit.paramOneAPI, out int percentage))
      {
          return false;
      }

      return percentage >= 0 && percentage <= 100;
    }


    public bool IsDeductionTypeValid(CompanyBenefitDTO benefit)
    {
      if (string.IsNullOrWhiteSpace(benefit.benefit.deductionType))
      {
        return false;
      }
      var deductionType = benefit.benefit.deductionType;

      var opcionesValidas = new List<string>
      {
        "porcentaje",
        "montoFijo",
        "api"
      };
      return opcionesValidas.Contains(deductionType);
    }

  }
}