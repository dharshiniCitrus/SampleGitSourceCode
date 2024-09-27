namespace EmployeeAccess.Model
{
    public class LogEntry
    {
        public int Id { get; set; }
        //public DateTime Timestamp { get; set; }
       
        public string? LogMessage { get; set; }
        public string? LogDescription { get; set; }

        public string? LogEnvironment { get; set; }
    }
}
