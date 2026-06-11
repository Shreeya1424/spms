using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;

namespace spms.Controllers
{
   
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // GET: /Role
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Roles";
            ViewData["Title"] = "Manage Roles";
            return View(DummyDataService.Roles);
        }

        // GET: /Role/Create
        public IActionResult Create()
        {
            ViewData["ActivePage"] = "Roles";
            ViewData["Title"] = "Add Role";
            return View(new RoleModel());
        }

        // POST: /Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                // Auto-generate the next ID
                model.Id = DummyDataService.GetNextId(DummyDataService.Roles);
                DummyDataService.Roles.Add(model);
                TempData["SuccessMessage"] = "Role added successfully!";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Roles";
            ViewData["Title"] = "Add Role";
            return View(model);
        }

        // GET: /Role/Edit/1
        public IActionResult Edit(int id)
        {
            var role = DummyDataService.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Roles";
            ViewData["Title"] = "Edit Role";
            return View(role);
        }

        // POST: /Role/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = DummyDataService.Roles.FirstOrDefault(r => r.Id == model.Id);
                if (existing != null)
                {
                    // Update fields
                    existing.RoleName = model.RoleName;
                    existing.Description = model.Description;
                    TempData["SuccessMessage"] = "Role updated successfully!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Roles";
            ViewData["Title"] = "Edit Role";
            return View(model);
        }

        // GET: /Role/Delete/1
        public IActionResult Delete(int id)
        {
            var role = DummyDataService.Roles.FirstOrDefault(r => r.Id == id);
            if (role != null)
            {
                DummyDataService.Roles.Remove(role);
                TempData["SuccessMessage"] = "Role deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Role not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
