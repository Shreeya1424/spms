using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;

namespace spms.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: /User
        public IActionResult Index(string? search, string? roleFilter, string? statusFilter)
        {
            var users = DummyDataService.Users.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u =>
                    u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    u.Mobile.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Apply role filter
            if (!string.IsNullOrEmpty(roleFilter))
            {
                users = users.Where(u => u.Role == roleFilter);
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter))
            {
                users = users.Where(u => u.Status == statusFilter);
            }

            // Pass filter values back to view for persistence
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentRoleFilter"] = roleFilter;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["Roles"] = DummyDataService.Roles.Select(r => r.RoleName).ToList();
            ViewData["ActivePage"] = "Users";
            ViewData["Title"] = "Manage Users";

            return View(users.ToList());
        }

        // GET: /User/Create
        public IActionResult Create()
        {
            ViewData["ActivePage"] = "Users";
            ViewData["Title"] = "Add User";
            ViewData["Roles"] = DummyDataService.Roles.Where(r => r.RoleName != "Admin").Select(r => r.RoleName).ToList();
            return View(new UserModel());
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel model)
        {
            if (model.Role == "Admin")
            {
                ModelState.AddModelError("Role", "Cannot add another Admin. Only Faculty and Student roles are allowed.");
            }

            if (ModelState.IsValid)
            {
                model.Id = DummyDataService.GetNextId(DummyDataService.Users);
                DummyDataService.Users.Add(model);
                TempData["SuccessMessage"] = "User added successfully!";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Users";
            ViewData["Title"] = "Add User";
            ViewData["Roles"] = DummyDataService.Roles.Where(r => r.RoleName != "Admin").Select(r => r.RoleName).ToList();
            return View(model);
        }

        // GET: /User/Edit/1
        public IActionResult Edit(int id)
        {
            var user = DummyDataService.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Users";
            ViewData["Title"] = "Edit User";
            
            // If the user being edited is Admin, they can only stay Admin. Otherwise, they can be Student or Faculty.
            if (user.Role == "Admin")
                ViewData["Roles"] = new List<string> { "Admin" };
            else
                ViewData["Roles"] = DummyDataService.Roles.Where(r => r.RoleName != "Admin").Select(r => r.RoleName).ToList();

            return View(user);
        }

        // POST: /User/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel model)
        {
            var existing = DummyDataService.Users.FirstOrDefault(u => u.Id == model.Id);
            
            if (existing != null && existing.Role != "Admin" && model.Role == "Admin")
            {
                ModelState.AddModelError("Role", "Cannot assign Admin role.");
            }

            if (ModelState.IsValid)
            {
                if (existing != null)
                {
                    existing.FullName = model.FullName;
                    existing.Email = model.Email;
                    existing.Mobile = model.Mobile;
                    existing.Role = model.Role;
                    existing.Status = model.Status;
                    
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        existing.Password = model.Password;
                    }

                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            ViewData["ActivePage"] = "Users";
            ViewData["Title"] = "Edit User";
            if (existing != null && existing.Role == "Admin")
                ViewData["Roles"] = new List<string> { "Admin" };
            else
                ViewData["Roles"] = DummyDataService.Roles.Where(r => r.RoleName != "Admin").Select(r => r.RoleName).ToList();

            return View(model);
        }

        // GET: /User/Delete/1
        public IActionResult Delete(int id)
        {
            var user = DummyDataService.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                DummyDataService.Users.Remove(user);
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
