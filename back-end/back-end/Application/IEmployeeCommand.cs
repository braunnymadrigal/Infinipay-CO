using back_end.Domain;

namespace back_end.Application
{
    public interface IEmployeeCommand
    {
        public void UpdateEmployeeData(EmployeeModel employee, Guid id);
    }
}
