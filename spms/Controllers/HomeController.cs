using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;
using System.Security.Claims;

namespace spms.Controllers
{
    
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            // Get user's assigned projects and tasks based on role
            var projects = DummyDataService.Projects.AsEnumerable();
            var tasks = DummyDataService.Tasks.AsEnumerable();

            if (role == "Faculty")
            {
                var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Email == userEmail);
                if (faculty != null)
                {
                    var assignedProjectIds = DummyDataService.ProjectAssignments
                        .Where(a => a.AssignedToId == faculty.Id && a.AssignedToType == "Faculty")
                        .Select(a => a.ProjectId).Distinct().ToList();
                    projects = projects.Where(p => assignedProjectIds.Contains(p.Id));
                    tasks = tasks.Where(t => assignedProjectIds.Contains(t.ProjectId ?? 0));
                }
            }
            else if (role == "Student")
            {
                var student = DummyDataService.Students.FirstOrDefault(s => s.Email == userEmail);
                if (student != null)
                {
                    var assignedProjectIds = DummyDataService.ProjectAssignments
                        .Where(a => a.AssignedToId == student.Id && a.AssignedToType == "Student")
                        .Select(a => a.ProjectId).Distinct().ToList();
                    projects = projects.Where(p => assignedProjectIds.Contains(p.Id));
                    tasks = tasks.Where(t => t.AssignedToId == student.Id && t.AssignedToType == "Student");
                }
            }

            var projectList = projects.ToList();
            var taskList = tasks.ToList();

            var model = new DashboardViewModel
            {
                TotalUsers = DummyDataService.Users.Count,
                TotalStudents = DummyDataService.Students.Count,
                TotalFaculty = DummyDataService.Faculty.Count,
                TotalProjects = projectList.Count,
                TotalTasks = taskList.Count,

                // Project status distribution for chart
                ProjectsNotStarted = projectList.Count(p => p.Status == "Not Started"),
                ProjectsInProgress = projectList.Count(p => p.Status == "In Progress"),
                ProjectsCompleted = projectList.Count(p => p.Status == "Completed"),

                // Task priority distribution for chart
                TasksLow = taskList.Count(t => t.Priority == "Low"),
                TasksMedium = taskList.Count(t => t.Priority == "Medium"),
                TasksHigh = taskList.Count(t => t.Priority == "High"),
                TasksCritical = taskList.Count(t => t.Priority == "Critical")
            };

            ViewData["UserRole"] = role;
            ViewData["ActivePage"] = "Dashboard";
            ViewData["Title"] = "Dashboard";
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
