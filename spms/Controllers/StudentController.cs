using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;

namespace spms.Controllers
{
   
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {
        // GET: /Student
        public IActionResult Index(string? search, string? departmentFilter)
        {
            var students = DummyDataService.Students.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s =>
                    s.StudentName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    s.EnrollmentNo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    s.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Apply department filter
            if (!string.IsNullOrEmpty(departmentFilter))
            {
                students = students.Where(s => s.Department == departmentFilter);
            }

            // Get distinct departments for filter dropdown
            ViewData["Departments"] = DummyDataService.Students.Select(s => s.Department).Distinct().ToList();
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentDepartmentFilter"] = departmentFilter;
            ViewData["ActivePage"] = "Students";
            ViewData["Title"] = "Manage Students";

            return View(students.ToList());
        }

        // GET: /Student/Create
        public IActionResult Create()
        {
            ViewData["ActivePage"] = "Students";
            ViewData["Title"] = "Add Student";
            return View(new StudentModel());
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = DummyDataService.GetNextId(DummyDataService.Students);
                DummyDataService.Students.Add(model);
                TempData["SuccessMessage"] = "Student added successfully!";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Students";
            ViewData["Title"] = "Add Student";
            return View(model);
        }

        // GET: /Student/Edit/1
        public IActionResult Edit(int id)
        {
            var student = DummyDataService.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Students";
            ViewData["Title"] = "Edit Student";
            return View(student);
        }

        // POST: /Student/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = DummyDataService.Students.FirstOrDefault(s => s.Id == model.Id);
                if (existing != null)
                {
                    existing.StudentName = model.StudentName;
                    existing.EnrollmentNo = model.EnrollmentNo;
                    existing.Department = model.Department;
                    existing.Email = model.Email;
                    existing.Mobile = model.Mobile;
                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Students";
            ViewData["Title"] = "Edit Student";
            return View(model);
        }

        // GET: /Student/Delete/1
        public IActionResult Delete(int id)
        {
            var student = DummyDataService.Students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                DummyDataService.Students.Remove(student);
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Student not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
