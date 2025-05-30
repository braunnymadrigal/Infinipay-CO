namespace back_end.Application
{
    public interface IGrossSalary
    {
        bool CheckDateRangeCorrectness(DateOnly startDate, DateOnly endDate);
    }
}
