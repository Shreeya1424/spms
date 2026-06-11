using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;
using System.Security.Claims;

namespace spms.Controllers
{
    
    [Authorize]
    public class ScoreController : Controller
    {
        // GET: /Score
        public IActionResult Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            var scores = DummyDataService.Scores.AsEnumerable();

            if (role == "Faculty")
            {
                var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Email == userEmail);
                if (faculty != null)
                    scores = scores.Where(s => s.ScoredByFacultyId == faculty.Id);
            }
            else if (role == "Student")
            {
                var student = DummyDataService.Students.FirstOrDefault(s => s.Email == userEmail);
                if (student != null)
                    scores = scores.Where(s => s.StudentId == student.Id);
            }

            var scoreList = scores.ToList();
            foreach (var s in scoreList)
            {
                s.ProjectTitle = DummyDataService.Projects.FirstOrDefault(p => p.Id == s.ProjectId)?.ProjectTitle ?? "N/A";
                s.StudentName = DummyDataService.Students.FirstOrDefault(st => st.Id == s.StudentId)?.StudentName ?? "N/A";
                s.FacultyName = DummyDataService.Faculty.FirstOrDefault(f => f.Id == s.ScoredByFacultyId)?.FacultyName ?? "N/A";
            }

            ViewData["UserRole"] = role;
            ViewData["ActivePage"] = "Scores";
            ViewData["Title"] = role == "Student" ? "My Scores & Remarks" : "Manage Scores";
            return View(scoreList);
        }

        // GET: /Score/Create
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            // Faculty sees only their assigned projects, Admin sees all
            if (role == "Faculty")
            {
                var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Email == userEmail);
                if (faculty != null)
                {
                    var assignedProjectIds = DummyDataService.ProjectAssignments
                        .Where(a => a.AssignedToId == faculty.Id && a.AssignedToType == "Faculty")
                        .Select(a => a.ProjectId).Distinct().ToList();
                    ViewData["Projects"] = DummyDataService.Projects.Where(p => assignedProjectIds.Contains(p.Id)).ToList();
                }
            }
            else
            {
                ViewData["Projects"] = DummyDataService.Projects;
            }

            ViewData["Students"] = DummyDataService.Students;
            ViewData["ProjectAssignments"] = DummyDataService.ProjectAssignments;
            ViewData["ActivePage"] = "Scores";
            ViewData["Title"] = "Assign Score";
            return View(new ScoreModel());
        }

        // POST: /Score/Create
        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ScoreModel model)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            // Auto-set faculty ID
            if (role == "Faculty")
            {
                var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Email == userEmail);
                if (faculty != null) model.ScoredByFacultyId = faculty.Id;
            }

            model.ScoredDate = DateTime.Today;

            if (ModelState.IsValid)
            {
                model.Id = DummyDataService.GetNextId(DummyDataService.Scores);
                DummyDataService.Scores.Add(model);
                TempData["SuccessMessage"] = "Score assigned successfully!";
                return RedirectToAction("Index");
            }

            ViewData["Projects"] = DummyDataService.Projects;
            ViewData["Students"] = DummyDataService.Students;
            ViewData["ProjectAssignments"] = DummyDataService.ProjectAssignments;
            ViewData["ActivePage"] = "Scores";
            ViewData["Title"] = "Assign Score";
            return View(model);
        }

        // GET: /Score/Edit/1
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Edit(int id)
        {
            var score = DummyDataService.Scores.FirstOrDefault(s => s.Id == id);
            if (score == null) { TempData["ErrorMessage"] = "Score not found."; return RedirectToAction("Index"); }

            ViewData["Projects"] = DummyDataService.Projects;
            ViewData["Students"] = DummyDataService.Students;
            ViewData["ProjectAssignments"] = DummyDataService.ProjectAssignments;
            ViewData["ActivePage"] = "Scores";
            ViewData["Title"] = "Edit Score";
            return View(score);
        }

        // POST: /Score/Edit
        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ScoreModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = DummyDataService.Scores.FirstOrDefault(s => s.Id == model.Id);
                if (existing != null)
                {
                    existing.ProjectId = model.ProjectId;
                    existing.StudentId = model.StudentId;
                    existing.Score = model.Score;
                    existing.Remarks = model.Remarks;
                    existing.ScoredDate = DateTime.Today;
                    TempData["SuccessMessage"] = "Score updated successfully!";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "Score not found.";
                return RedirectToAction("Index");
            }

            ViewData["Projects"] = DummyDataService.Projects;
            ViewData["Students"] = DummyDataService.Students;
            ViewData["ProjectAssignments"] = DummyDataService.ProjectAssignments;
            ViewData["ActivePage"] = "Scores";
            ViewData["Title"] = "Edit Score";
            return View(model);
        }

        // GET: /Score/Delete/1
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Delete(int id)
        {
            var score = DummyDataService.Scores.FirstOrDefault(s => s.Id == id);
            if (score != null) { DummyDataService.Scores.Remove(score); TempData["SuccessMessage"] = "Score deleted!"; }
            else TempData["ErrorMessage"] = "Score not found.";
            return RedirectToAction("Index");
        }
    }
}
