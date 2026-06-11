using System.ComponentModel.DataAnnotations;

namespace spms.Models
{
   
    public class RoleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Role Name is required.")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}
