namespace spms.Models
{
    
    public class ProjectAssignmentModel
    {
        public int ProjectId { get; set; }
        public int AssignedToId { get; set; }
        public string AssignedToType { get; set; } = string.Empty; // "Student" or "Faculty"

        
        public string? AssignedToName { get; set; }

       
        public int TaskCount { get; set; }
    }
}
