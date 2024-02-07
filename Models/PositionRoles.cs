using System.ComponentModel.DataAnnotations;

namespace CarbonFootprint1.Models
{
    public class PositionRoles
    {
        [Key]
        public int PosiitonRoleId { get; set; }
        public string? Postion { get; set; }
        public string? Roles { get; set; }
    }
}
