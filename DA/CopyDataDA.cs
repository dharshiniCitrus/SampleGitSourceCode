using EmployeeAccess.Model;
using System.Data.SqlClient;
using System.Data;
using EmployeeAccess.BL;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace EmployeeAccess.DA
{
    public class CopyDataDA
    {
        public static string connectionString = "";
        public static string connectionString1 = "Data Source = PRIYADHARSHINI\\SQLEXPRESS; Initial Catalog = Employee; User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;";
        public static string sourcedatabaseString = "";
        public static string targetdatabaseString = "";
        public static string sourcetable = "";
        public static string targettable = "";
        public static string msg = "";
        public static string status = "";


        public static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //Save Data into Another Database
        public static string DataCopy(CopyData copyData)
        {

            try
            {
                if (copyData.sourcedatabase != "")
                {
                    sourcedatabaseString = copyData.sourcedatabase;

                    //Check if Source Database Exists
                    if (CheckDatabaseExists(sourcedatabaseString) == false)
                    {
                        msg = "Source Database Doesn't Exists. Please create a Source Database";

                        return msg;

                    }
                    else
                    {
                        connectionString = "Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Initial Catalog = " + sourcedatabaseString + ";Trusted_Connection=True;";
                    }

                }
                else
                {
                    msg = "Please mention the source database";
                }
                if (copyData.targetdatabase != "")
                {
                    targetdatabaseString = copyData.targetdatabase;

                    //Check If Target Database Exists
                    if (CheckDatabaseExists(targetdatabaseString) == false)
                    {
                        msg = "Target Database Doesn't Exists. Please create a  Target Database";

                        return msg;

                    }
                    else
                    {
                        connectionString1 = "Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Initial Catalog = " + targetdatabaseString + ";Trusted_Connection=True;";
                    }
                }
                else
                {
                    msg = "Please mention the target database";
                }
                if (copyData.sourcetable != "")
                {
                    sourcedatabaseString = copyData.sourcedatabase;
                    sourcetable = copyData.sourcetable;

                    SqlConnection tmpConn = new SqlConnection("Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;");


                    //Check if Source Table Exists
                    if (TableExists(tmpConn, sourcedatabaseString, sourcetable) == false)
                    {
                        msg = "Source Table Doesn't Exists in Source Database. Please create a Source Table.";

                        return msg;

                    }
                }
                else
                {
                    msg = "Please mention the source table";
                }
                if (copyData.targettable != "")
                {
                    targettable = copyData.targettable;
                    SqlConnection tmpConn = new SqlConnection("Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;");


                    //Check if Source Table Exists
                    if (TableExists(tmpConn, targetdatabaseString, targettable) == false)
                    {
                        msg = "Target Table Doesn't Exists in Target Database. Please create a Source Table.";

                        return msg;

                    }
                }
                else
                {
                    msg = "Please mention the target table";
                }

                //Check If both the Target Database Table and Source Database Table Contains Same Datatypes and parameters
                bool CheckDataValues = CheckSourceandTargetTableValues(copyData);
                if (CheckDataValues == false)
                {
                    msg = "Target Table and Source Table Contains Different ColumnName and DataTypes ";
                    return msg;

                }
                else
                {
                    //Get Data from Sourcedatabase table
                    List<Employee> EmployeeDetails = EmployeeDA.EmpListAll(connectionString, sourcetable);

                    //Check if Target Database table has Records
                    int TargetDataCount = GetCountofTargetDataInserted(copyData);
                    if (TargetDataCount > 0)
                    {
                        //Delete the records to avoid Duplication in the Target Database Table
                        DeleteDuplicateRecords(copyData);
                    }

                    if (EmployeeDetails != null && EmployeeDetails.Count > 0)
                    {

                        for (int i = 0; i <= EmployeeDetails.Count() - 1; i++)
                        {
                            int rows = 0;
                            //Insert into target Database table
                            using (SqlConnection con = new SqlConnection(connectionString1))
                            {
                                SqlCommand cmd = new SqlCommand(@"Insert Into " + targettable + "" +
                                    "(Id, Name, Department, Status)" +
                    "Values(@Id, @Name, @Department, @Status)"
                                , con);

                                cmd.Parameters.AddWithValue("@Id", EmployeeDetails[i].Id);
                                cmd.Parameters.AddWithValue("@Name", EmployeeDetails[i].Name);
                                cmd.Parameters.AddWithValue("@Department", EmployeeDetails[i].Department);
                                cmd.Parameters.AddWithValue("@Status", EmployeeDetails[i].Status);

                                cmd.CommandType = CommandType.Text;
                                con.Open();
                                rows = cmd.ExecuteNonQuery();

                                con.Close();
                            }

                        }

                        msg = "Data Saved Successfully";
                    }
                    else
                    {
                        msg = "Source Table Data is Empty";
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env.ToString();
                LogEntryBL.LogSave(logEntry);
                msg = ex.ToString();
            }



            return msg;

            

        }

        private static bool CheckSourceandTargetTableValues(CopyData copyData)
        {
            try
            {
                string connectionstring="Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Initial Catalog = " + copyData.sourcedatabase + ";Trusted_Connection=True;";
                

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetTableColumnsAndTypes", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Assuming copyData.sourcedatabase and copyData.targetdatabase are correctly defined.
                    cmd.Parameters.AddWithValue("@SourceDb", SqlDbType.Int).Value = copyData.sourcedatabase;
                    cmd.Parameters.AddWithValue("@SourceTable", SqlDbType.VarChar).Value = copyData.sourcetable; // Assuming this should be a string
                    cmd.Parameters.AddWithValue("@TargetDb", SqlDbType.Int).Value = copyData.targetdatabase;
                    cmd.Parameters.AddWithValue("@TargetTable", SqlDbType.VarChar).Value = copyData.targettable; // Assuming this should be a string

                    con.Open();

                    // Use ExecuteReader directly for reading the data
                    //SqlDataReader reader = cmd.ExecuteReader();

                    DataSet ds = new DataSet();
                    //ds.Load(reader, LoadOption.PreserveChanges, "ColumnsInfo");

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(ds);

                   // reader.Close();

                    // Check if both tables contain the same values and columns
                    if (ds.Tables.Count > 0)
                    {
                        DataTable sourceTable = ds.Tables[0]; // Assuming the first table is the source
                        DataTable targetTable = ds.Tables[1]; // Assuming the second table is the target

                        if (AreTablesTheSame(ds.Tables[0], ds.Tables[1]) == false)
                        {

                            return false;
                        }
                        else
                        {
                            return true;
                        }
                        
                    }
                    
                }

               
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env.ToString();
                LogEntryBL.LogSave(logEntry);
            }

            return true;
        }

        //Compare Source and Target Tables
        public static bool AreTablesTheSame(DataTable tbl1, DataTable tbl2)
        {
            try
            {
                if (tbl1.Rows.Count != tbl2.Rows.Count || tbl1.Columns.Count != tbl2.Columns.Count)
                    return false;


                for (int i = 0; i < tbl1.Rows.Count; i++)
                {
                    for (int c = 0; c < tbl1.Columns.Count; c++)
                    {
                        if (!Equals(tbl1.Rows[i][c], tbl2.Rows[i][c]))
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env.ToString(); 
                LogEntryBL.LogSave(logEntry);
            }
            return true;
        }

        //Delete Duplicate Records in Target Table
        private static string DeleteDuplicateRecords(CopyData copyData)
        {
            connectionString1 = "Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Initial Catalog = " + copyData.targetdatabase + ";Trusted_Connection=True;";
            msg = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString1))
                {
                    SqlCommand cmd = new SqlCommand("Delete From " + copyData.targettable + "; ", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }
            catch(Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env.ToString();
                LogEntryBL.LogSave(logEntry);
            }
            
            return msg;
        }

        //Get Count of Records in the Target table
        public static int GetCountofTargetDataInserted(CopyData copyData)
        {
            int rows = 0;
            try
            {
                
                connectionString1 = "Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Initial Catalog = " + copyData.targetdatabase + ";Trusted_Connection=True;";
                List<Employee> EmployeeDetails = EmployeeDA.EmpListAll(connectionString1, targettable);

                if (EmployeeDetails != null && EmployeeDetails.Count > 0)
                {
                    rows = EmployeeDetails.Count();

                }
               
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env.ToString();
                LogEntryBL.LogSave(logEntry);
                return 0;
            }
            return rows;
        }

        //Check if Database Exists
        private static bool CheckDatabaseExists(string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                SqlConnection tmpConn = new SqlConnection("Data Source = PRIYADHARSHINI\\SQLEXPRESS;User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;");

                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
        
                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env.ToString();
                LogEntryBL.LogSave(logEntry);
            }

            return result;
        }

        //Check if Table Exists
        private static bool TableExists(SqlConnection conn, string database, string name)
        {
            string strCmd = null;
            SqlCommand sqlCmd = null;
            bool result = false;
            try
            {
                strCmd = "select case when exists((select '['+SCHEMA_NAME(schema_id)+'].['+name+']' As name FROM [" + database + "].sys.tables WHERE name = '" + name + "')) then 1 else 0 end";
                conn.Open();
                sqlCmd = new SqlCommand(strCmd, conn);

                object resultObj = sqlCmd.ExecuteScalar();

                int TableID = 0;

                if (resultObj != null)
                {
                    int.TryParse(resultObj.ToString(), out TableID);
                }

                conn.Close();

                result = (TableID > 0);


               
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env.ToString();
                LogEntryBL.LogSave(logEntry);
                result = false; 
            }

            return result;
        }

    }
}
