using EmployeeAccess.DA;
using EmployeeAccess.Model;

namespace EmployeeAccess.BL
{
    public class EmployeeBL
    {
        public static int Save(Employee employee)
        {
            return EmployeeDA.Save(employee);
        }

        public static List<Employee> ListAll()
        {
            return EmployeeDA.ListAll();
        }

        public static Employee ListById(int id)
        {
            return EmployeeDA.ListById(id);
        }
    }
}
