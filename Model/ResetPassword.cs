namespace EmployeeAccess.Model
{
    public class ResetPassword
    {
        public string? newPassword { get; set; }

        public string? confirmPassword { get; set; }

        public string? resetToken { get; set; }
    }
}
