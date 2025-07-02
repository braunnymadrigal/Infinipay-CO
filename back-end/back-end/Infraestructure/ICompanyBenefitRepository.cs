using back_end.Models;
using System;
using System.Collections.Generic;

namespace back_end.Repositories
{
    public interface ICompanyBenefitRepository
    {
        List<CompanyBenefitDTO> getBenefits(string nickname);
        CompanyBenefitDTO getBenefitById(Guid id);
        bool CreateBenefit(CompanyBenefitDTO companyBenefit, string loggedUserNickname);
        void UpdateBenefit(Guid id, CompanyBenefitDTO companyBenefit, string loggedUserNickname);
        void DeleteBenefit(Guid id, string loggedUserNickname);
        List<string> getEmployeesWithBenefit(Guid id);
    }
}
