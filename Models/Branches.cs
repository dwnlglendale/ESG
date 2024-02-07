using System.ComponentModel.DataAnnotations;

namespace CarbonFootprint1.Models
{
    public class Branches
    {
        [Key]
        public int BranchID { get; set; }

        public string? BraanchCode { get; set; }

        public string? BranchName { get; set; }

        public string? BranchSize { get; set; }
    }
}
