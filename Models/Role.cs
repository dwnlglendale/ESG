using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarbonFootprint1.Models
{
    public class Role : IdentityRole
    {
        
        public string? Description { get; set; }
        [ValidateNever]
        public DateTime DateCreated { get; set; }
        [ValidateNever]
        public string? CreatedBy { get; set; }
        [ValidateNever]
        public string? Status { get; set; }
    }
}
