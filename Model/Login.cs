namespace EmployeeAccess.Model
{
    public class Login
    {
        public int Id { get; set; }
        public string? username { get; set; }
        
        public string? password { get; set; }

        public string? Token { get; set; }

    }
}
