using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class EmployeeCommand : IEmployeeCommand
    {
        private readonly EmployeeRepository _employeeRepository;
        public EmployeeCommand()
        {
            _employeeRepository = new EmployeeRepository();
        }
        
        public void UpdateEmployeeData(EmployeeModel employee, Guid id, string loggedUsername)
        {
            try {
                _employeeRepository.UpdateEmployeeData(employee, id, loggedUsername);
            }
            catch (Exception ex) {
                if (ex.Message.Contains("CEDULA_DUPLICADA"))
                    throw new Exception("Ya existe un empleado registrado con esa cédula.");
                if (ex.Message.Contains("TELEFONO_DUPLICADO"))
                    throw new Exception("Ya existe un empleado registrado con ese número de teléfono.");
                if (ex.Message.Contains("EMAIL_DUPLICADO"))
                    throw new Exception("Ya existe un empleado registrado con ese correo electrónico.");
                if (ex.Message.Contains("USERNAME_DUPLICADO"))
                    throw new Exception("Ya existe un empleado registrado con ese nombre de usuario.");
                if (ex.Message.Contains("EMPLOYEE_NOT_FOUND"))
                    throw new Exception("Empleado no encontrado.");
                if (ex.Message.Contains("EMPLOYEE_ALREADY_PAYED"))
                    throw new Exception("El empleado ya ha sido pagado.");
                throw new Exception("Error inesperado actualizando empleado." +
                    " Detalles: " + ex.Message);
            }
        }

    }
}
