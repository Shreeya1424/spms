using System.ComponentModel.DataAnnotations;

namespace spms.Models
{
    
    public class ProjectModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project Title is required.")]
        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(3);

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Not Started";

       
        [Display(Name = "Supervising Faculty")]
        public int? FacultyId { get; set; }

        
        [Display(Name = "Assigned Students")]
        public List<int> StudentIds { get; set; } = new List<int>();

        [Display(Name = "Assigned To")]
        public string? AssignedToName { get; set; }

        
        public List<ProjectAssignmentModel> AssignedMembers { get; set; } = new List<ProjectAssignmentModel>();
    }
}
