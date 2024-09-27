using EmployeeAccess.DA;
using EmployeeAccess.Model;

namespace EmployeeAccess.BL
{
    public class LogEntryBL
    {
        public static int LogSave(LogEntry logEntry)
        {
            return LogEntryDA.LogSave(logEntry);
        }
    }
}
