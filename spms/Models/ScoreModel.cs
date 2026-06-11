using System.ComponentModel.DataAnnotations;

namespace spms.Models
{
    
    public class ScoreModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
        [Display(Name = "Score")]
        public int Score { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; } = string.Empty;

        [Display(Name = "Scored By (Faculty)")]
        public int ScoredByFacultyId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime ScoredDate { get; set; } = DateTime.Today;

        // Display-only properties
        public string? ProjectTitle { get; set; }
        public string? StudentName { get; set; }
        public string? FacultyName { get; set; }
    }
}
