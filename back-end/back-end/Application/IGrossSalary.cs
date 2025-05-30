namespace back_end.Application
{
    public interface IGrossSalary
    {
        void SetDateRange(DateOnly startDate, DateOnly endDate);
        void CheckDateRangeCorrectness();
    }
}
