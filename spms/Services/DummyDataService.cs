using spms.Models;

namespace spms.Services
{
    
    public static class DummyDataService
    {
        // ==================== ROLES ====================
        public static List<RoleModel> Roles = new List<RoleModel>
        {
            new RoleModel { Id = 1, RoleName = "Admin", Description = "Full access to all modules and settings." },
            new RoleModel { Id = 2, RoleName = "Student", Description = "Can view and manage own projects and tasks." },
            new RoleModel { Id = 3, RoleName = "Faculty", Description = "Can supervise students and manage project assignments." }
        };

        // ==================== USERS ====================
        public static List<UserModel> Users = new List<UserModel>
        {
            new UserModel { Id = 1, FullName = "Aarav Patel", Email = "admin@spms.com", Password = "admin123", Mobile = "9876543210", Role = "Admin", Status = "Active" },
            new UserModel { Id = 2, FullName = "Priya Sharma", Email = "priya.sharma@spms.com", Password = "faculty123", Mobile = "9876543211", Role = "Faculty", Status = "Active" },
            new UserModel { Id = 3, FullName = "Rohan Mehta", Email = "rohan.mehta@spms.com", Password = "student123", Mobile = "9876543212", Role = "Student", Status = "Active" },
            new UserModel { Id = 4, FullName = "Sneha Desai", Email = "sneha.desai@spms.com", Password = "student123", Mobile = "9876543213", Role = "Student", Status = "Active" },
            new UserModel { Id = 5, FullName = "Vikram Singh", Email = "vikram.singh@spms.com", Password = "faculty123", Mobile = "9876543214", Role = "Faculty", Status = "Inactive" },
            new UserModel { Id = 6, FullName = "Anita Joshi", Email = "anita.joshi@spms.com", Password = "student123", Mobile = "9876543215", Role = "Student", Status = "Active" }
        };

        // ==================== STUDENTS ====================
        public static List<StudentModel> Students = new List<StudentModel>
        {
            new StudentModel { Id = 1, StudentName = "Rohan Mehta", EnrollmentNo = "ENR2024001", Department = "Computer Science", Email = "rohan.mehta@spms.com", Mobile = "9876543212" },
            new StudentModel { Id = 2, StudentName = "Sneha Desai", EnrollmentNo = "ENR2024002", Department = "Information Technology", Email = "sneha.desai@spms.com", Mobile = "9876543213" },
            new StudentModel { Id = 3, StudentName = "Anita Joshi", EnrollmentNo = "ENR2024003", Department = "Computer Science", Email = "anita.joshi@spms.com", Mobile = "9876543215" },
            new StudentModel { Id = 4, StudentName = "Rahul Verma", EnrollmentNo = "ENR2024004", Department = "Electronics", Email = "rahul.verma@spms.com", Mobile = "9876543217" },
            new StudentModel { Id = 5, StudentName = "Meera Nair", EnrollmentNo = "ENR2024005", Department = "Mechanical", Email = "meera.nair@spms.com", Mobile = "9876543218" },
            new StudentModel { Id = 6, StudentName = "Arjun Reddy", EnrollmentNo = "ENR2024006", Department = "Information Technology", Email = "arjun.reddy@spms.com", Mobile = "9876543219" }
        };

        // ==================== FACULTY ====================
        public static List<FacultyModel> Faculty = new List<FacultyModel>
        {
            new FacultyModel { Id = 1, FacultyName = "Priya Sharma", Department = "Computer Science", Email = "priya.sharma@spms.com", Mobile = "9876543211" },
            new FacultyModel { Id = 2, FacultyName = "Vikram Singh", Department = "Information Technology", Email = "vikram.singh@spms.com", Mobile = "9876543214" },
            new FacultyModel { Id = 3, FacultyName = "Dr. Nisha Kapoor", Department = "Electronics", Email = "nisha.kapoor@spms.com", Mobile = "9876543220" },
            new FacultyModel { Id = 4, FacultyName = "Prof. Rajesh Kumar", Department = "Mechanical", Email = "rajesh.kumar@spms.com", Mobile = "9876543221" }
        };

        // ==================== PERMISSIONS ====================
        public static List<PermissionModel> Permissions = new List<PermissionModel>
        {
            new PermissionModel { Id = 1, PermissionName = "Create Project", Description = "Ability to create new projects." },
            new PermissionModel { Id = 2, PermissionName = "Edit Project", Description = "Ability to edit existing projects." },
            new PermissionModel { Id = 3, PermissionName = "Delete Project", Description = "Ability to delete projects." },
            new PermissionModel { Id = 4, PermissionName = "Manage Tasks", Description = "Ability to create, edit, and delete tasks." },
            new PermissionModel { Id = 5, PermissionName = "View Reports", Description = "Ability to view reports and analytics." },
            new PermissionModel { Id = 6, PermissionName = "Manage Users", Description = "Ability to manage user accounts." }
        };

        // ==================== ROLE-PERMISSION MAPPING ====================
        public static List<RolePermissionModel> RolePermissions = new List<RolePermissionModel>
        {
            // Admin has all permissions
            new RolePermissionModel { RoleId = 1, PermissionId = 1, IsGranted = true },
            new RolePermissionModel { RoleId = 1, PermissionId = 2, IsGranted = true },
            new RolePermissionModel { RoleId = 1, PermissionId = 3, IsGranted = true },
            new RolePermissionModel { RoleId = 1, PermissionId = 4, IsGranted = true },
            new RolePermissionModel { RoleId = 1, PermissionId = 5, IsGranted = true },
            new RolePermissionModel { RoleId = 1, PermissionId = 6, IsGranted = true },
            // Student has limited permissions
            new RolePermissionModel { RoleId = 2, PermissionId = 1, IsGranted = false },
            new RolePermissionModel { RoleId = 2, PermissionId = 2, IsGranted = false },
            new RolePermissionModel { RoleId = 2, PermissionId = 3, IsGranted = false },
            new RolePermissionModel { RoleId = 2, PermissionId = 4, IsGranted = true },
            new RolePermissionModel { RoleId = 2, PermissionId = 5, IsGranted = true },
            new RolePermissionModel { RoleId = 2, PermissionId = 6, IsGranted = false },
            // Faculty has moderate permissions
            new RolePermissionModel { RoleId = 3, PermissionId = 1, IsGranted = true },
            new RolePermissionModel { RoleId = 3, PermissionId = 2, IsGranted = true },
            new RolePermissionModel { RoleId = 3, PermissionId = 3, IsGranted = false },
            new RolePermissionModel { RoleId = 3, PermissionId = 4, IsGranted = true },
            new RolePermissionModel { RoleId = 3, PermissionId = 5, IsGranted = true },
            new RolePermissionModel { RoleId = 3, PermissionId = 6, IsGranted = false }
        };

        // ==================== PROJECTS ====================
        public static List<ProjectModel> Projects = new List<ProjectModel>
        {
            new ProjectModel { Id = 1, ProjectTitle = "E-Commerce Platform", Description = "Build a full-stack e-commerce web application.", StartDate = new DateTime(2024, 9, 1), EndDate = new DateTime(2025, 3, 1), Status = "In Progress" },
            new ProjectModel { Id = 2, ProjectTitle = "Library Management System", Description = "Develop a library management system with barcode scanning.", StartDate = new DateTime(2024, 8, 15), EndDate = new DateTime(2025, 2, 15), Status = "Completed" },
            new ProjectModel { Id = 3, ProjectTitle = "AI Chatbot", Description = "Create an AI-powered chatbot for student queries.", StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 6, 30), Status = "In Progress" },
            new ProjectModel { Id = 4, ProjectTitle = "IoT Weather Station", Description = "Build an IoT-based weather monitoring station.", StartDate = new DateTime(2025, 3, 1), EndDate = new DateTime(2025, 9, 1), Status = "Not Started" },
            new ProjectModel { Id = 5, ProjectTitle = "Mobile Attendance App", Description = "Android app for biometric attendance tracking.", StartDate = new DateTime(2025, 2, 1), EndDate = new DateTime(2025, 7, 31), Status = "In Progress" }
        };

        // ==================== TASKS ====================
        public static List<ProjectTaskModel> Tasks = new List<ProjectTaskModel>
        {
            new ProjectTaskModel { Id = 1, TaskTitle = "Design Database Schema", Description = "Create ER diagram and normalize tables.", Priority = "High", Status = "Completed", AssignedToId = 1, AssignedToType = "Student", DueDate = new DateTime(2024, 9, 15), ProjectId = 1 },
            new ProjectTaskModel { Id = 2, TaskTitle = "Setup Project Structure", Description = "Initialize MVC project with folder structure.", Priority = "Medium", Status = "Completed", AssignedToId = 1, AssignedToType = "Student", DueDate = new DateTime(2024, 9, 20), ProjectId = 1 },
            new ProjectTaskModel { Id = 3, TaskTitle = "Implement User Authentication", Description = "Add login/register functionality.", Priority = "High", Status = "In Progress", AssignedToId = 1, AssignedToType = "Student", DueDate = new DateTime(2024, 10, 1), ProjectId = 1 },
            new ProjectTaskModel { Id = 4, TaskTitle = "Create Book Catalog Module", Description = "CRUD for books with search functionality.", Priority = "Medium", Status = "Completed", AssignedToId = 2, AssignedToType = "Student", DueDate = new DateTime(2024, 10, 15), ProjectId = 2 },
            new ProjectTaskModel { Id = 5, TaskTitle = "Train NLP Model", Description = "Train the chatbot NLP model with sample data.", Priority = "Critical", Status = "In Progress", AssignedToId = 1, AssignedToType = "Faculty", DueDate = new DateTime(2025, 3, 1), ProjectId = 3 },
            new ProjectTaskModel { Id = 6, TaskTitle = "Design UI Wireframes", Description = "Create wireframes for all app screens.", Priority = "Low", Status = "Pending", AssignedToId = 3, AssignedToType = "Student", DueDate = new DateTime(2025, 2, 15), ProjectId = 5 },
            new ProjectTaskModel { Id = 7, TaskTitle = "Sensor Integration", Description = "Connect temperature and humidity sensors.", Priority = "High", Status = "Pending", AssignedToId = 4, AssignedToType = "Student", DueDate = new DateTime(2025, 4, 1), ProjectId = 4 },
            new ProjectTaskModel { Id = 8, TaskTitle = "API Development", Description = "Build REST APIs for the e-commerce platform.", Priority = "High", Status = "In Progress", AssignedToId = 1, AssignedToType = "Student", DueDate = new DateTime(2024, 11, 1), ProjectId = 1 }
        };

        // ==================== PROJECT ASSIGNMENTS (Multiple members per project) ====================
        public static List<ProjectAssignmentModel> ProjectAssignments = new List<ProjectAssignmentModel>
        {
            // E-Commerce Platform — 3 students + 1 faculty
            new ProjectAssignmentModel { ProjectId = 1, AssignedToId = 1, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 1, AssignedToId = 3, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 1, AssignedToId = 6, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 1, AssignedToId = 1, AssignedToType = "Faculty" },

            // Library Management System — 2 students
            new ProjectAssignmentModel { ProjectId = 2, AssignedToId = 2, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 2, AssignedToId = 5, AssignedToType = "Student" },

            // AI Chatbot — 1 faculty + 2 students
            new ProjectAssignmentModel { ProjectId = 3, AssignedToId = 1, AssignedToType = "Faculty" },
            new ProjectAssignmentModel { ProjectId = 3, AssignedToId = 4, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 3, AssignedToId = 1, AssignedToType = "Student" },

            // IoT Weather Station — 2 students + 1 faculty
            new ProjectAssignmentModel { ProjectId = 4, AssignedToId = 4, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 4, AssignedToId = 2, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 4, AssignedToId = 3, AssignedToType = "Faculty" },

            // Mobile Attendance App — 2 students
            new ProjectAssignmentModel { ProjectId = 5, AssignedToId = 3, AssignedToType = "Student" },
            new ProjectAssignmentModel { ProjectId = 5, AssignedToId = 6, AssignedToType = "Student" }
        };

       
        public static List<ProjectAssignmentModel> GetProjectAssignees(int projectId)
        {
            var assignments = ProjectAssignments
                .Where(a => a.ProjectId == projectId)
                .ToList();

            foreach (var a in assignments)
            {
                a.AssignedToName = GetAssigneeName(a.AssignedToId, a.AssignedToType);
                // Count tasks assigned to this member in this project
                a.TaskCount = Tasks.Count(t =>
                    t.ProjectId == projectId &&
                    t.AssignedToId == a.AssignedToId &&
                    t.AssignedToType == a.AssignedToType);
            }

            return assignments;
        }

        // ==================== SCORES ====================
        public static List<ScoreModel> Scores = new List<ScoreModel>
        {
            new ScoreModel { Id = 1, ProjectId = 1, StudentId = 1, Score = 85, Remarks = "Excellent work on database design. Good understanding of normalization.", ScoredByFacultyId = 1, ScoredDate = new DateTime(2025, 1, 15) },
            new ScoreModel { Id = 2, ProjectId = 1, StudentId = 3, Score = 72, Remarks = "Good progress but needs improvement in API development.", ScoredByFacultyId = 1, ScoredDate = new DateTime(2025, 1, 15) },
            new ScoreModel { Id = 3, ProjectId = 2, StudentId = 2, Score = 91, Remarks = "Outstanding implementation. Clean code and well-documented.", ScoredByFacultyId = 1, ScoredDate = new DateTime(2025, 2, 20) },
            new ScoreModel { Id = 4, ProjectId = 3, StudentId = 4, Score = 68, Remarks = "Needs to focus more on NLP model accuracy.", ScoredByFacultyId = 1, ScoredDate = new DateTime(2025, 3, 10) },
            new ScoreModel { Id = 5, ProjectId = 3, StudentId = 1, Score = 78, Remarks = "Good contribution to chatbot training data collection.", ScoredByFacultyId = 1, ScoredDate = new DateTime(2025, 3, 10) },
            new ScoreModel { Id = 6, ProjectId = 5, StudentId = 3, Score = 88, Remarks = "Creative UI designs. Excellent wireframe quality.", ScoredByFacultyId = 2, ScoredDate = new DateTime(2025, 3, 1) },
            new ScoreModel { Id = 7, ProjectId = 4, StudentId = 4, Score = 75, Remarks = "Sensor integration is progressing well.", ScoredByFacultyId = 3, ScoredDate = new DateTime(2025, 4, 5) }
        };

        // ====================  Get Next ID ====================
       
        public static int GetNextId<T>(List<T> list) where T : class
        {
            if (list.Count == 0) return 1;

            // Use reflection to get the Id property value
            var maxId = list.Max(item =>
            {
                var prop = typeof(T).GetProperty("Id");
                return prop != null ? (int)(prop.GetValue(item) ?? 0) : 0;
            });
            return maxId + 1;
        }

        // ==================== Get Assignee Name ====================
       
        public static string GetAssigneeName(int? assignedToId, string assignedToType)
        {
            if (assignedToId == null || string.IsNullOrEmpty(assignedToType))
                return "Unassigned";

            if (assignedToType == "Student")
            {
                var student = Students.FirstOrDefault(s => s.Id == assignedToId);
                return student?.StudentName ?? "Unknown Student";
            }
            else if (assignedToType == "Faculty")
            {
                var faculty = Faculty.FirstOrDefault(f => f.Id == assignedToId);
                return faculty?.FacultyName ?? "Unknown Faculty";
            }

            return "Unassigned";
        }
    }
}
