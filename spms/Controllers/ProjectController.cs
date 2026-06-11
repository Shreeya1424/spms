using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;
using System.Security.Claims;

namespace spms.Controllers
{
    
    [Authorize]
    public class ProjectController : Controller
    {
        public IActionResult Index(string? search, string? statusFilter)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            var projects = DummyDataService.Projects.AsEnumerable();

            // Role-based filtering: Faculty/Student only see their assigned projects
            if (role == "Faculty")
            {
                // Find faculty ID by email
                var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Email == userEmail);
                if (faculty != null)
                {
                    var assignedProjectIds = DummyDataService.ProjectAssignments
                        .Where(a => a.AssignedToId == faculty.Id && a.AssignedToType == "Faculty")
                        .Select(a => a.ProjectId).Distinct().ToList();
                    projects = projects.Where(p => assignedProjectIds.Contains(p.Id));
                }
            }
            else if (role == "Student")
            {
                // Find student ID by email
                var student = DummyDataService.Students.FirstOrDefault(s => s.Email == userEmail);
                if (student != null)
                {
                    var assignedProjectIds = DummyDataService.ProjectAssignments
                        .Where(a => a.AssignedToId == student.Id && a.AssignedToType == "Student")
                        .Select(a => a.ProjectId).Distinct().ToList();
                    projects = projects.Where(p => assignedProjectIds.Contains(p.Id));
                }
            }

            if (!string.IsNullOrEmpty(search))
                projects = projects.Where(p => p.ProjectTitle.Contains(search, StringComparison.OrdinalIgnoreCase) || p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(statusFilter))
                projects = projects.Where(p => p.Status == statusFilter);

            var projectList = projects.ToList();
            foreach (var p in projectList)
            {
                // p.AssignedMembers is used by the view for displaying Faculty and Students

                p.AssignedMembers = DummyDataService.GetProjectAssignees(p.Id);
            }

            ViewData["UserRole"] = role;
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["ActivePage"] = "Projects";
            ViewData["Title"] = role == "Admin" ? "Manage Projects" : "My Projects";
            return View(projectList);
        }

        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            PopulateDropdowns();
            ViewData["ActivePage"] = "Projects";
            ViewData["Title"] = "Add Project";
            return View(new ProjectModel());
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = DummyDataService.GetNextId(DummyDataService.Projects);
                DummyDataService.Projects.Add(model);

                // Add Faculty assignment
                if (model.FacultyId.HasValue)
                {
                    DummyDataService.ProjectAssignments.Add(new ProjectAssignmentModel
                    {
                        ProjectId = model.Id,
                        AssignedToId = model.FacultyId.Value,
                        AssignedToType = "Faculty"
                    });
                }
                
                // Add Student assignments
                if (model.StudentIds != null && model.StudentIds.Any())
                {
                    foreach (var studentId in model.StudentIds)
                    {
                        DummyDataService.ProjectAssignments.Add(new ProjectAssignmentModel
                        {
                            ProjectId = model.Id,
                            AssignedToId = studentId,
                            AssignedToType = "Student"
                        });
                    }
                }

                TempData["SuccessMessage"] = "Project added successfully!";
                return RedirectToAction("Index");
            }
            PopulateDropdowns();
            ViewData["ActivePage"] = "Projects";
            ViewData["Title"] = "Add Project";
            return View(model);
        }

        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Edit(int id)
        {
            var project = DummyDataService.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null) { TempData["ErrorMessage"] = "Project not found."; return RedirectToAction("Index"); }
            PopulateDropdowns();
            ViewData["ActivePage"] = "Projects";
            ViewData["Title"] = "Edit Project";
            // Load current assignments into the model for the view
            var assignments = DummyDataService.ProjectAssignments.Where(a => a.ProjectId == id).ToList();
            project.FacultyId = assignments.FirstOrDefault(a => a.AssignedToType == "Faculty")?.AssignedToId;
            project.StudentIds = assignments.Where(a => a.AssignedToType == "Student").Select(a => a.AssignedToId).ToList();

            return View(project);
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                var e = DummyDataService.Projects.FirstOrDefault(p => p.Id == model.Id);
                if (e != null)
                {
                    e.ProjectTitle = model.ProjectTitle; e.Description = model.Description;
                    e.StartDate = model.StartDate; e.EndDate = model.EndDate;
                    e.Status = model.Status;
                    e.FacultyId = model.FacultyId;
                    e.StudentIds = model.StudentIds ?? new List<int>();

                    // Sync assignment list: Remove old assignments for this project
                    DummyDataService.ProjectAssignments.RemoveAll(a => a.ProjectId == model.Id);

                    // Re-add Faculty assignment
                    if (model.FacultyId.HasValue)
                    {
                        DummyDataService.ProjectAssignments.Add(new ProjectAssignmentModel
                        {
                            ProjectId = model.Id,
                            AssignedToId = model.FacultyId.Value,
                            AssignedToType = "Faculty"
                        });
                    }
                    
                    // Re-add Student assignments
                    if (model.StudentIds != null && model.StudentIds.Any())
                    {
                        foreach (var studentId in model.StudentIds)
                        {
                            DummyDataService.ProjectAssignments.Add(new ProjectAssignmentModel
                            {
                                ProjectId = model.Id,
                                AssignedToId = studentId,
                                AssignedToType = "Student"
                            });
                        }
                    }

                    TempData["SuccessMessage"] = "Project updated successfully!";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "Project not found.";
                return RedirectToAction("Index");
            }
            PopulateDropdowns();
            ViewData["ActivePage"] = "Projects";
            ViewData["Title"] = "Edit Project";
            return View(model);
        }

        // POST: /Project/UpdateStatus — inline status update from Index page
        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            var project = DummyDataService.Projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                project.Status = status;
                TempData["SuccessMessage"] = $"Project status updated to '{status}'.";
            }
            else
            {
                TempData["ErrorMessage"] = "Project not found.";
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Delete(int id)
        {
            var p = DummyDataService.Projects.FirstOrDefault(p => p.Id == id);
            if (p != null) { DummyDataService.Projects.Remove(p); TempData["SuccessMessage"] = "Project deleted successfully!"; }
            else TempData["ErrorMessage"] = "Project not found.";
            return RedirectToAction("Index");
        }

        private void PopulateDropdowns()
        {
            ViewData["Students"] = DummyDataService.Students;
            ViewData["Faculty"] = DummyDataService.Faculty;
        }
    }
}
