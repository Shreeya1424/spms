using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;

namespace spms.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
    {
        // GET: /Faculty
        public IActionResult Index(string? search, string? departmentFilter)
        {
            var faculty = DummyDataService.Faculty.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                faculty = faculty.Where(f =>
                    f.FacultyName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    f.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Apply department filter
            if (!string.IsNullOrEmpty(departmentFilter))
            {
                faculty = faculty.Where(f => f.Department == departmentFilter);
            }

            ViewData["Departments"] = DummyDataService.Faculty.Select(f => f.Department).Distinct().ToList();
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentDepartmentFilter"] = departmentFilter;
            ViewData["ActivePage"] = "Faculty";
            ViewData["Title"] = "Manage Faculty";

            return View(faculty.ToList());
        }

        // GET: /Faculty/Create
        public IActionResult Create()
        {
            ViewData["ActivePage"] = "Faculty";
            ViewData["Title"] = "Add Faculty";
            return View(new FacultyModel());
        }

        // POST: /Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = DummyDataService.GetNextId(DummyDataService.Faculty);
                DummyDataService.Faculty.Add(model);
                TempData["SuccessMessage"] = "Faculty added successfully!";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Faculty";
            ViewData["Title"] = "Add Faculty";
            return View(model);
        }

        // GET: /Faculty/Edit/1
        public IActionResult Edit(int id)
        {
            var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Id == id);
            if (faculty == null)
            {
                TempData["ErrorMessage"] = "Faculty not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Faculty";
            ViewData["Title"] = "Edit Faculty";
            return View(faculty);
        }

        // POST: /Faculty/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = DummyDataService.Faculty.FirstOrDefault(f => f.Id == model.Id);
                if (existing != null)
                {
                    existing.FacultyName = model.FacultyName;
                    existing.Department = model.Department;
                    existing.Email = model.Email;
                    existing.Mobile = model.Mobile;
                    TempData["SuccessMessage"] = "Faculty updated successfully!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Faculty not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Faculty";
            ViewData["Title"] = "Edit Faculty";
            return View(model);
        }

        // GET: /Faculty/Delete/1
        public IActionResult Delete(int id)
        {
            var faculty = DummyDataService.Faculty.FirstOrDefault(f => f.Id == id);
            if (faculty != null)
            {
                DummyDataService.Faculty.Remove(faculty);
                TempData["SuccessMessage"] = "Faculty deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Faculty not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
