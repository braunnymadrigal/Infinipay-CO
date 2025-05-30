using back_end.Infraestructure;

namespace back_end.Application
{
    public class GrossSalary : IGrossSalary
    {
        private const int MAXIMUM_NUMBER_OF_DAYS_A_DATE_RANGE_CAN_REPRESENT = 31;
        private const int WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 7;
        private const int BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 15;
        private const int MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 30;

        private DateOnly startDate;
        private DateOnly endDate;
        private int numberOfWorkedDays;
        private readonly IGrossSalaryRepository grossSalaryRepository;

        public GrossSalary()
        {
            grossSalaryRepository = new GrossSalaryRepository();
        }

        public void SetDateRange(DateOnly startDate, DateOnly endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public void CheckDateRangeCorrectness()
        {
            if (startDate == DateOnly.MinValue || endDate == DateOnly.MaxValue)
            {
                throw new Exception("Date values are not coherent.");
            }
            if (startDate >= endDate)
            {
                throw new Exception("The start date shall not surpass the end date.");
            }
            if (startDate.AddDays(MAXIMUM_NUMBER_OF_DAYS_A_DATE_RANGE_CAN_REPRESENT) < endDate)
            {
                throw new Exception("The range of date shall not represent more than one month");
            }
        }

        public void SetNumberOfWorkedDays()
        {
            var rawNumberOfDays = endDate.DayNumber - startDate.DayNumber;
            numberOfWorkedDays = rawNumberOfDays <= BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK 
                ? BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK : MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK;
            if (rawNumberOfDays <= WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                numberOfWorkedDays = WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK;
            }
        }
    }
}
