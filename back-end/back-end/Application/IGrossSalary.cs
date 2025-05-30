namespace back_end.Application
{
    public interface IGrossSalary
    {
        void SetDateRange(DateOnly startDate, DateOnly endDate);
        void CheckDateRangeCorrectness();
        void SetNumberOfWorkedDays();
        void SetIdEmployer(string id);
        void ComputeAllGrossSalaries();
    }
}
