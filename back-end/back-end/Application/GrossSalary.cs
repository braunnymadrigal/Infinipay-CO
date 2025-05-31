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

        public GrossSalary()
        {
            grossSalaryRepository = new GrossSalaryRepository();
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
            List<GrossSalaryModel> grossSalaries;
            grossSalaries = grossSalaryRepository.GetGrossSalaries(idEmployer);
            PrintGrossSalaryModelList(grossSalaries);
            if (numberOfWorkedDays == WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                //
            }
            if (numberOfWorkedDays == BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                grossSalaries = ComputeBiweeklySalaries(grossSalaries);
            }
            if (numberOfWorkedDays == MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                grossSalaries = ComputeMonthlySalaries(grossSalaries);
            }
            PrintGrossSalaryModelList(grossSalaries);
        }

        //private List<GrossSalaryModel> ComputeWeeklySalaries(List<GrossSalaryModel> grossSalaries)
        //{
        //    for (int i = 0; i < grossSalaries.Count; ++i)
        //    {
        //        grossSalaries[i].GrossSalary = grossSalaries[i].GrossSalary / 2;
        //        if (grossSalaries[i].HiringDate > endDate)
        //        {
        //            grossSalaries[i].EmployeeId = "";
        //        }
        //        else
        //        {
        //            if (grossSalaries[i].HiringDate > startDate)
        //            {
        //                var numberOfWorkedDays = (endDate.Day - grossSalaries[i].HiringDate.Day) + 1;
        //                var newGrossSalary = (grossSalaries[i].GrossSalary / BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK) * numberOfWorkedDays;
        //                grossSalaries[i].GrossSalary = newGrossSalary;
        //            }
        //        }
        //    }
        //    return grossSalaries;
        //}

        private List<GrossSalaryModel> ComputeBiweeklySalaries(List<GrossSalaryModel> grossSalaries)
        {
            for (int i = 0; i < grossSalaries.Count; ++i)
            {
                grossSalaries[i].GrossSalary = grossSalaries[i].GrossSalary / 2;
                if (grossSalaries[i].HiringDate > endDate)
                {
                    grossSalaries[i].EmployeeId = "";
                }
                else
                {
                    if (grossSalaries[i].HiringDate > startDate)
                    {
                        var numberOfWorkedDays = (endDate.Day - grossSalaries[i].HiringDate.Day) + 1;
                        var newGrossSalary = (grossSalaries[i].GrossSalary / BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK) * numberOfWorkedDays;
                        grossSalaries[i].GrossSalary = newGrossSalary;
                    }
                }
            }
            return grossSalaries;
        }

        private List<GrossSalaryModel> ComputeMonthlySalaries(List<GrossSalaryModel> grossSalaries)
        {
            for (int i = 0; i < grossSalaries.Count; ++i)
            {
                if (grossSalaries[i].HiringDate > endDate)
                {
                    grossSalaries[i].EmployeeId = "";
                }
                else
                {
                    if (grossSalaries[i].HiringDate > startDate)
                    {
                        var numberOfWorkedDays = (endDate.Day - grossSalaries[i].HiringDate.Day) + 1;
                        var newGrossSalary = (grossSalaries[i].GrossSalary / MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK) * numberOfWorkedDays;
                        grossSalaries[i].GrossSalary = newGrossSalary;
                    }
                }
            }
            return grossSalaries;
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
