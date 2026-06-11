using System.ComponentModel.DataAnnotations;

namespace spms.Models
{
    
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Role")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";
    }
}
