namespace EmployeeAccess.Model
{
  
        public class ValidationSummary
        {
            //To set the summary is valid or not. Default value need to set as true and if any validation can set it as false.
            public bool IsValid { get; set; }
            //To set messages as list
            public List<ValidationMessage> Messages { get; set; }
        }

        public class ValidationMessage
        {
            //Type of the message.
            public ValidationType Type { get; set; }
            //Validation message
            public string Message { get; set; }
        }

        public enum ValidationType
        {
            INVALID,
            VALID,
            WARNING,
            ERROR,
            UNKNOWN
            //We can add more types based on requirements
        }
    
}
