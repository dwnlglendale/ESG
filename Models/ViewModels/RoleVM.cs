namespace CarbonFootprint1.Models.ViewModels
{
    public class RoleVM
    {
        public RoleInputModel? RoleInput { get; set; }
        public Role? Roles { get; set; }

    }


    public class RoleInputModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public string? Status { get; set; }
      
    }
}
