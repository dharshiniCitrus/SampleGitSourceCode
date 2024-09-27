using EmployeeAccess.Model;
using System.Data.SqlClient;
using System.Data;

namespace EmployeeAccess.DA
{
    public class LogEntryDA(LogEntry logEntry)
    {
        public static string connectionString = "Data Source = PRIYADHARSHINI\\SQLEXPRESS; Initial Catalog = Sample; User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;";
        public static int LogSave(LogEntry logEntry)
        {
            int rows = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"Insert Into LogEntry(LogMessage,LogDescription,LogEnvironment) 
                    Values(@LogMessage, @LogDescription, @LogEnvironment)", con);

               
                cmd.Parameters.AddWithValue("@LogMessage", logEntry.LogMessage);
                cmd.Parameters.AddWithValue("@LogDescription", logEntry.LogDescription);
                cmd.Parameters.AddWithValue("@LogEnvironment", logEntry.LogEnvironment);

                cmd.CommandType = CommandType.Text;
                con.Open();
                rows = cmd.ExecuteNonQuery();

                con.Close();
            }
            return rows;

        }
    }
}
