using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarbonFootprint1.Models.ViewModels
{
    public class UserVM
    {
        public UserInputModel UserInput { get; set; }

        [ValidateNever]
        public List<SelectListItem> RoleList { get; set; }

        [ValidateNever]
        public ApplicationUser AppUser { get; set; }

        public string Role { get; set; }
    }

    public class UserInputModel
    {
        public int Id { get; set; }

        [RegularExpression("^[A-Za-z.\\-]+$", ErrorMessage = "Input must only contain letters and dots")]
        public string UserName { get; set; }

        public string Branch { get; set; }

        public string BranchCode { get; set; }

        public string BranchSize { get; set; }

        public string StaffID { get; set; }

        public string UserPosition { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string BDSelection { get; set; }
    }
}
