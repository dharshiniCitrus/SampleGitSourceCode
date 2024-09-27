namespace EmployeeAccess.Model
{
    public class JWTInformation
    {
        public string SecretKey { get; set; }
        public int ExpiryHour { get; set; }
        public int OrgAdminRoleId { get; set; }
        public string OrgAdminRoleName { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretId { get; set; }
        public string TableauTokenMailHost { get; set; }

    }
}
