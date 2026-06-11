using System.ComponentModel.DataAnnotations;

namespace spms.Models
{
    
    public class ProjectTaskModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task Title is required.")]
        [Display(Name = "Task Title")]
        public string TaskTitle { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priority is required.")]
        [Display(Name = "Priority")]
        public string Priority { get; set; } = "Medium";

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// ID of the assigned Student or Faculty member.
        /// </summary>
        [Display(Name = "Assigned To")]
        public int? AssignedToId { get; set; }

        /// <summary>
        /// Type of assignee: "Student" or "Faculty".
        /// </summary>
        [Display(Name = "Assigned Type")]
        public string AssignedToType { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the assigned person (computed for display).
        /// </summary>
        [Display(Name = "Assigned To")]
        public string? AssignedToName { get; set; }

        [Required(ErrorMessage = "Due Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(7);

        [Display(Name = "Project")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Project title for display purposes.
        /// </summary>
        [Display(Name = "Project")]
        public string? ProjectTitle { get; set; }
    }
}
