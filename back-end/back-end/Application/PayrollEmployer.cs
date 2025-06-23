using System.Xml.Serialization;
using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class PayrollEmployer : IPayrollEmployer
    {
        private const string BIWEEKLY_PAYMENT_TYPE = "quincenal";
        private const string MONTHLY_PAYMENT_TYPE = "mensual";

        private const int BIWEEKLY_FIRST_HALF_START_DATE = 1;
        private const int BIWEEKLY_SECOND_HALF_START_DATE = 16;
        private const int BIWEEKLY_FIRST_HALF_END_DATE = 15;
        private const int MONTHLY_START_DATE = 1;

        private readonly IPayrollEmployerRepository _payrollEmployerRepository;

        public PayrollEmployer(IPayrollEmployerRepository payrollEmployerRepository)
        {
            _payrollEmployerRepository = payrollEmployerRepository;
        }

        public PayrollEmployerModel getPayrollEmployer(string id, DateOnly startDate, DateOnly endDate)
        {
            var payrollEmployer = new PayrollEmployerModel
            {
                id = id,
                startDate = startDate,
                endDate = endDate,
            };
            payrollEmployer = _payrollEmployerRepository.getPayrollEmployer(payrollEmployer);
            checkStringNotEmpty(payrollEmployer.paymentType);
            payrollEmployer.latestEndDate = _payrollEmployerRepository.getDateOfLatestPayroll(payrollEmployer.companyId);
            checkDateRangeCorrectness(payrollEmployer);
            return payrollEmployer;
        }

        public string getPaymentType(string id)
        {
            var payrollEmployer = new PayrollEmployerModel
            {
                id = id,
            };
            payrollEmployer = _payrollEmployerRepository.getPayrollEmployer(payrollEmployer);
            checkStringNotEmpty(payrollEmployer.paymentType);
            return payrollEmployer.paymentType;
        }

        private void checkDateRangeCorrectness(PayrollEmployerModel payrollEmployer)
        {
            checkDateRangeLimits(payrollEmployer.startDate, payrollEmployer.endDate);
            checkEndDateSurpassStartDate(payrollEmployer.startDate, payrollEmployer.endDate);
            checkDateRangeIsInTheSamePeriod(payrollEmployer.startDate, payrollEmployer.endDate);
            checkDateRangeIsInThePastOrPresent(payrollEmployer.startDate, payrollEmployer.endDate);
            checkStartDateIsAfterLatestEndDate(payrollEmployer.startDate, payrollEmployer.latestEndDate);
            switch (payrollEmployer.paymentType)
            {
                case BIWEEKLY_PAYMENT_TYPE:
                    checkBiweeklyDateRangeCorrectness(payrollEmployer.startDate, payrollEmployer.endDate);
                    break;
                case MONTHLY_PAYMENT_TYPE:
                    checkMonthlyDateRangeCorrectness(payrollEmployer.startDate, payrollEmployer.endDate);
                    break;
                default:
                    throw new Exception("PayrollEmployer: Payment type not supported.");
            }
        }

        private void checkEndDateSurpassStartDate(DateOnly startDate, DateOnly endDate)
        {
            if (startDate >= endDate)
            {
                throw new Exception("PayrollEmployer: start date must be before end date.");
            }
        }

        private void checkDateRangeLimits(DateOnly startDate, DateOnly endDate)
        {
            if (startDate == DateOnly.MinValue)
            {
                throw new Exception("PayrollEmployer: start date is equal to DateOnly min value.");
            }
            if (endDate == DateOnly.MaxValue)
            {
                throw new Exception("PayrollEmployer: end date is equal to DateOnly max value.");
            }
        }

        private void checkDateRangeIsInTheSamePeriod(DateOnly startDate, DateOnly endDate)
        {
            checkDateRangeIsInTheSameYear(startDate, endDate);
            checkDateRangeIsInTheSameMonth(startDate, endDate);
        }

        private void checkDateRangeIsInTheSameYear(DateOnly startDate, DateOnly endDate)
        {
            if (startDate.Year != endDate.Year)
            {
                throw new Exception("PayrollEmployer: Date range is not in the same year.");
            }
        }

        private void checkDateRangeIsInTheSameMonth(DateOnly startDate, DateOnly endDate)
        {
            if (startDate.Month != endDate.Month)
            {
                throw new Exception("PayrollEmployer: Date range is not in the same month.");
            }
        }

        private void checkDateRangeIsInThePastOrPresent(DateOnly startDate, DateOnly endDate)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            if (startDate.Year > currentDate.Year)
            {
                throw new Exception("PayrollEmployer: Date range year is in the future");
            }
            if (startDate.Month > currentDate.Month)
            {
                throw new Exception("PayrollEmployer: Date range month is in the future");
            }
        }

        private void checkStartDateIsAfterLatestEndDate(DateOnly startDate, DateOnly latestEndDate)
        {
            if (latestEndDate != DateOnly.MinValue)
            {
                if (startDate != latestEndDate.AddDays(1))
                {
                    throw new Exception("PayrollEmployer: Start date should be 1 day after latest end date.");
                }
            }
        }

        private void checkBiweeklyDateRangeCorrectness(DateOnly startDate, DateOnly endDate)
        {
            switch (startDate.Day)
            {
                case BIWEEKLY_FIRST_HALF_START_DATE:
                    checkBiweeklyFirstHalfEndDate(endDate);
                    break;
                case BIWEEKLY_SECOND_HALF_START_DATE:
                    checkEndDateIsTheLastDayOfTheMonth(endDate);
                    break;
                default:
                    throw new Exception("PayrollEmployer: Biweekly start date not supported.");

            }
        }

        private void checkMonthlyDateRangeCorrectness(DateOnly startDate, DateOnly endDate)
        {
            switch (startDate.Day)
            {
                case MONTHLY_START_DATE:
                    checkEndDateIsTheLastDayOfTheMonth(endDate);
                    break;
                default:
                    throw new Exception("PayrollEmployer: Monthly start date not supported.");
            }
        }

        private void checkBiweeklyFirstHalfEndDate(DateOnly endDate)
        {
            if (endDate.Day != BIWEEKLY_FIRST_HALF_END_DATE)
            {
                throw new Exception("PayrollEmployer: first half of month must end the 15.");
            }
        }

        private void checkEndDateIsTheLastDayOfTheMonth(DateOnly endDate)
        {
            if (endDate.Day != DateTime.DaysInMonth(endDate.Year, endDate.Month))
            {
                throw new Exception("PayrollEmployer: end date must be the last day of the month.");
            }
        }

        private void checkStringNotEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("PayrollEmployer: caught invalid string.");
            }
        }
    }
}
