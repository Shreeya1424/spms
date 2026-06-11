using System.ComponentModel.DataAnnotations;

namespace spms.Models
{
   
    public class FacultyModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Faculty Name is required.")]
        [Display(Name = "Faculty Name")]
        public string FacultyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; } = string.Empty;
    }
}
