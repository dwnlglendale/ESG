using System.ComponentModel.DataAnnotations;

namespace CarbonFootprint1.Models
{
    public class Departments
    {
        [Key]
        public int DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        public string? DepartmentValue { get; set; }
    }
}
