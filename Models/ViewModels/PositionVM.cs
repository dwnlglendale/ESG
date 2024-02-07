using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarbonFootprint1.Models.ViewModels
{
    public class PositionVM
    {
        public PositionInputModel? PositionInput { get; set; }
        public List<SelectListItem> RoleList { get; set; }

    }


    public class PositionInputModel
    {
        public int? Position_ID { get; set; }

        [Required(ErrorMessage = "Position Name is required.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Position Name should only contain alphabets.")]
        [DisplayName("Position Name")]
        public string? Position_Name { get; set; }

        [Required(ErrorMessage = "Position Description is required.")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Position Description should only contain alphabets.")]
        [DisplayName("Role Description")]
        public string? Position_Description { get; set; }
       
      
    }
}
