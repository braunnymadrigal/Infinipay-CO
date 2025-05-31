using back_end.Domain;

namespace back_end.Application
{
    public interface IGrossSalary
    {
        void SetDateRange(string startDate, string endDate);
        void SetNumberOfWorkedDays();
        void SetIdEmployer(string id);
        List<GrossSalaryModel> ComputeAllGrossSalaries();
    }
}
