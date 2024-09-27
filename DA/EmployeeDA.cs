using EmployeeAccess.Model;
using System.Data.SqlClient;
using System.Data;

namespace EmployeeAccess.DA
{
    public class EmployeeDA
    {
        
        private static string _connectionString = "Data Source = PRIYADHARSHINI\\SQLEXPRESS; Initial Catalog = Sample; User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;";

        public static int Save(Employee employee)
        {
            int rows = 0;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"Insert Into Employee(Id, Name, Department, Status) 
                    Values(@Id, @Name, @Department, @Status)", con);

                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@Status", employee.Status);

                cmd.CommandType = CommandType.Text;
                con.Open();
                rows = cmd.ExecuteNonQuery();

                con.Close();
            }
            return rows;

        }

        public static List<Employee> ListAll()
        {
            var employees = new List<Employee>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Select Id, Name, Department, Status From Employee", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(rdr[0]),
                            Name = rdr[1].ToString(),
                            Department = rdr[2].ToString(),
                            Status = rdr[3].ToString(),
                        });
                    }
                    rdr.Close();
                    con.Close();
                }
            }
            catch
            {
                throw new Exception("Something wrong in connection");
            }
            return employees;
        }


        //CopyData from Table1 employee

        public static List<Employee> EmpListAll(string _connectionString,string tablename)
        {
            var employees = new List<Employee>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Select Id, Name, Department, Status From " +tablename+ "; ", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(rdr[0]),
                            Name = rdr[1].ToString(),
                            Department = rdr[2].ToString(),
                            Status = rdr[3].ToString(),
                        });
                    }
                    rdr.Close();
                    con.Close();
                }
            }
            catch
            {
                throw new Exception("Something wrong in connection");
            }
            return employees;
        }

        public static Employee ListById(int id)
        {
            var employee = new Employee();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetEmployeeById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {

                        employee.Id = Convert.ToInt32(rdr[0]);
                        employee.Name = rdr[1].ToString();
                        employee.Department = rdr[2].ToString();
                        employee.Status = rdr[3].ToString();

                    }
                    rdr.Close();
                    con.Close();
                }
            }
            catch
            {
                throw new Exception("Something wrong in connection");
            }
            return employee;
        }
    }
}
