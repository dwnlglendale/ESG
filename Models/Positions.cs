using System.ComponentModel.DataAnnotations;

namespace CarbonFootprint1.Models
{
    public class Positions
    {
        [Key]
        public int Position_Id { get; set; }
        public string? Position_Name { get; set; }
        public string? Position_Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime DateCreated  { get; set; }

        public string? Position_Status { get; set; }
    }
}
