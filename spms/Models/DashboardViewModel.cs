namespace spms.Models
{
    
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalFaculty { get; set; }
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }

        // Chart data for project status distribution
        public int ProjectsNotStarted { get; set; }
        public int ProjectsInProgress { get; set; }
        public int ProjectsCompleted { get; set; }

        // Chart data for task priority distribution
        public int TasksLow { get; set; }
        public int TasksMedium { get; set; }
        public int TasksHigh { get; set; }
        public int TasksCritical { get; set; }
    }
}
