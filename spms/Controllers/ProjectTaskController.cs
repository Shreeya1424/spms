using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;
using System.Security.Claims;

namespace spms.Controllers
{
   
    [Authorize]
    public class ProjectTaskController : Controller
    {
        public IActionResult Index(string? search, string? priorityFilter, string? statusFilter)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            var tasks = DummyDataService.Tasks.AsEnumerable();

            // Role-based filtering
            if (role == "Faculty")
            {
                var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Email == userEmail);
                if (faculty != null)
                {
                    // Faculty sees tasks in projects they're assigned to
                    var assignedProjectIds = DummyDataService.ProjectAssignments
                        .Where(a => a.AssignedToId == faculty.Id && a.AssignedToType == "Faculty")
                        .Select(a => a.ProjectId).Distinct().ToList();
                    tasks = tasks.Where(t => assignedProjectIds.Contains(t.ProjectId ?? 0));
                }
            }
            else if (role == "Student")
            {
                var student = DummyDataService.Students.FirstOrDefault(s => s.Email == userEmail);
                if (student != null)
                {
                    // Student sees only their own assigned tasks
                    tasks = tasks.Where(t => t.AssignedToId == student.Id && t.AssignedToType == "Student");
                }
            }

            if (!string.IsNullOrEmpty(search))
                tasks = tasks.Where(t => t.TaskTitle.Contains(search, StringComparison.OrdinalIgnoreCase) || t.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(priorityFilter))
                tasks = tasks.Where(t => t.Priority == priorityFilter);
            if (!string.IsNullOrEmpty(statusFilter))
                tasks = tasks.Where(t => t.Status == statusFilter);

            var taskList = tasks.ToList();
            foreach (var t in taskList)
            {
                t.AssignedToName = DummyDataService.GetAssigneeName(t.AssignedToId, t.AssignedToType);
                t.ProjectTitle = DummyDataService.Projects.FirstOrDefault(p => p.Id == t.ProjectId)?.ProjectTitle ?? "N/A";
            }

            ViewData["UserRole"] = role;
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentPriorityFilter"] = priorityFilter;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["ActivePage"] = "Tasks";
            ViewData["Title"] = role == "Admin" ? "Manage Tasks" : "My Tasks";
            return View(taskList);
        }

        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            PopulateDropdowns();
            ViewData["ActivePage"] = "Tasks";
            ViewData["Title"] = "Add Task";
            return View(new ProjectTaskModel());
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ProjectTaskModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = DummyDataService.GetNextId(DummyDataService.Tasks);
                DummyDataService.Tasks.Add(model);
                TempData["SuccessMessage"] = "Task added successfully!";
                return RedirectToAction("Index");
            }
            PopulateDropdowns();
            ViewData["ActivePage"] = "Tasks";
            ViewData["Title"] = "Add Task";
            return View(model);
        }

        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Edit(int id)
        {
            var task = DummyDataService.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) { TempData["ErrorMessage"] = "Task not found."; return RedirectToAction("Index"); }
            PopulateDropdowns();
            ViewData["ActivePage"] = "Tasks";
            ViewData["Title"] = "Edit Task";
            return View(task);
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectTaskModel model)
        {
            if (ModelState.IsValid)
            {
                var e = DummyDataService.Tasks.FirstOrDefault(t => t.Id == model.Id);
                if (e != null)
                {
                    e.TaskTitle = model.TaskTitle; e.Description = model.Description;
                    e.Priority = model.Priority; e.Status = model.Status;
                    e.AssignedToId = model.AssignedToId; e.AssignedToType = model.AssignedToType;
                    e.DueDate = model.DueDate; e.ProjectId = model.ProjectId;
                    TempData["SuccessMessage"] = "Task updated successfully!";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction("Index");
            }
            PopulateDropdowns();
            ViewData["ActivePage"] = "Tasks";
            ViewData["Title"] = "Edit Task";
            return View(model);
        }

        // POST: /ProjectTask/UpdateStatus — inline status update from Index page
        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            var task = DummyDataService.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.Status = status;
                TempData["SuccessMessage"] = $"Task status updated to '{status}'.";
            }
            else
            {
                TempData["ErrorMessage"] = "Task not found.";
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Delete(int id)
        {
            var t = DummyDataService.Tasks.FirstOrDefault(t => t.Id == id);
            if (t != null) { DummyDataService.Tasks.Remove(t); TempData["SuccessMessage"] = "Task deleted successfully!"; }
            else TempData["ErrorMessage"] = "Task not found.";
            return RedirectToAction("Index");
        }

        private void PopulateDropdowns()
        {
            ViewData["Students"] = DummyDataService.Students;
            ViewData["Faculty"] = DummyDataService.Faculty;
            ViewData["Projects"] = DummyDataService.Projects;
            ViewData["ProjectAssignments"] = DummyDataService.ProjectAssignments;
        }
    }
}
