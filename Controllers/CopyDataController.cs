using EmployeeAccess.BL;
using EmployeeAccess.DA;
using EmployeeAccess.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeAccess.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class CopyDataController : ControllerBase
    {
        public string message = "";
        public static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [HttpPost("Save")]
        public IActionResult Save([FromBody] CopyData CopyData)
        {
            string Desc = "";
            try
            {
                message= CopyDataBL.DataCopy(CopyData).ToString();
                if (message != "Data Saved Successfully")
                {
                    
                    var response1 = new
                    {
                        status = "Failed",
                        description = message
                    };
                    return Ok(response1);
                }
                else
                {
                    int recordsInsertedintoTarget=Convert.ToInt32(CopyDataBL.GetCountofTargetDataInserted(CopyData));
                   
                    Desc = recordsInsertedintoTarget + " record(s) copied into Target Database table. " + message;

                    var response = new
                    {
                        status = "Success",

                        description= Desc
                    };
                    return Ok(response);
                }
                
               
            }
            catch(Exception ex)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.LogMessage = "Error";
                logEntry.LogDescription = ex.Message.ToString();
                logEntry.LogEnvironment = env;
                LogEntryBL.LogSave(logEntry);
                var response = new
                {
                    status = "Error in The Execution of Data Copy",
                    description = ex.Message.ToString()
                };
                return Ok(response);
                
            }

        }
    }
}
