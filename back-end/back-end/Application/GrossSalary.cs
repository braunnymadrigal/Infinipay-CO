using System.Collections.Generic;
using back_end.Domain;
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
        private string idEmployer;
        private readonly IGrossSalaryRepository grossSalaryRepository;
        private IContextGrossSalaryComputation contextGrossSalaryComputation;

        public GrossSalary(IGrossSalaryRepository grossSalaryRepository, IContextGrossSalaryComputation contextGrossSalaryComputation)
        {
            this.grossSalaryRepository = grossSalaryRepository;
            this.contextGrossSalaryComputation = contextGrossSalaryComputation;
            idEmployer = "";
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

        public void SetIdEmployer(string id)
        {
            idEmployer = id;
        }

        public void ComputeAllGrossSalaries()
        {
            var grossSalaries = grossSalaryRepository.GetGrossSalaries(idEmployer, startDate, endDate);
            grossSalaries = RemoveEmployeesThatShouldNotBeOnPayroll(grossSalaries);
            SetProperContextGrossSalaryComputation();
            grossSalaries = contextGrossSalaryComputation.ComputeGrossSalary(grossSalaries);
            PrintGrossSalaryModelList(grossSalaries);
        }

        private List<GrossSalaryModel> RemoveEmployeesThatShouldNotBeOnPayroll(List<GrossSalaryModel> grossSalaries)
        {
            for (int i = grossSalaries.Count - 1; i >= 0; --i)
            {
                if (grossSalaries[i].HiringDate > endDate)
                {
                    grossSalaries.RemoveAt(i);
                }
            }
            return grossSalaries;
        }

        private void SetProperContextGrossSalaryComputation()
        {
            contextGrossSalaryComputation.SetRangeOfDates(startDate, endDate);
            if (numberOfWorkedDays == WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                contextGrossSalaryComputation.SetStrategy(new WeeklyGrossSalaryComputation());
            }
            if (numberOfWorkedDays == BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                contextGrossSalaryComputation.SetStrategy(new BiweeklyGrossSalaryComputation());
            }
            if (numberOfWorkedDays == MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                contextGrossSalaryComputation.SetStrategy(new MonthlyGrossSalaryComputation());
            }
        }

        private void PrintGrossSalaryModelList(List<GrossSalaryModel> grossSalaryModels)
        {
            foreach (var grossSalaryModel in grossSalaryModels)
            {
                Console.WriteLine("------------------------------------------------------");
                PrintGrossSalaryModel(grossSalaryModel);
                Console.WriteLine("------------------------------------------------------");
            }
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------");
        }

        private void PrintGrossSalaryModel(GrossSalaryModel grossSalaryModel)
        {
            Console.WriteLine("\tEmployeeId: " + grossSalaryModel.EmployeeId);
            Console.WriteLine("\tHiringDate: " + grossSalaryModel.HiringDate);
            Console.WriteLine("\tGrossSalary: " + grossSalaryModel.GrossSalary);
            Console.WriteLine("\tHiringType: " + grossSalaryModel.HiringType);
            Console.WriteLine("\tHoursDate: " + grossSalaryModel.HoursDate);
            Console.WriteLine("\tHoursWorked: " + grossSalaryModel.HoursWorked);
        }
    }
}
