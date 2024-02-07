using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarbonFootprint1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? BranchName { get; set; }

        public string? BranchCode { get; set; }

        public string? BranchSize { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public string? UserPosition { get; set; }

        public string? StaffNumber { get; set; }
        public string? Department { get; set; }
        public string? Status { get; set; }
        public string? BDSelection { get; set; }


    }
}
