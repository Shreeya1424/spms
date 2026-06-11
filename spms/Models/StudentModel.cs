using System.ComponentModel.DataAnnotations;

namespace spms.Models
{
    
    public class StudentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enrollment No is required.")]
        [Display(Name = "Enrollment No")]
        public string EnrollmentNo { get; set; } = string.Empty;

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
