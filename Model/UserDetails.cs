using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAccess.Model
{
    public class UserDetails
    {
        public int? Id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public bool? isActive { get; set; }
        public string? createdOn { get; set; }
        public string? updatedOn { get; set; }

        public string? JWTToken { get; set; }
    }

    public class UserDetailsActive
    {
        public List<int> userIds { get; set; }
        public bool? isActive { get; set; }

    }


    }
