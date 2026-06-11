using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;

namespace spms.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class PermissionController : Controller
    {
        // GET: /Permission
        public IActionResult Index()
        {
            var viewModel = new RolePermissionViewModel
            {
                Roles = DummyDataService.Roles,
                Permissions = DummyDataService.Permissions,
                RolePermissions = DummyDataService.RolePermissions
            };

            ViewData["ActivePage"] = "Permissions";
            ViewData["Title"] = "Role & Permissions";
            return View(viewModel);
        }

        // POST: /Permission/Update
        // Toggles a single permission for a role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int roleId, int permissionId, bool isGranted)
        {
            var existing = DummyDataService.RolePermissions
                .FirstOrDefault(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (existing != null)
            {
                // Toggle the permission
                existing.IsGranted = isGranted;
            }
            else
            {
                // Create new mapping if it doesn't exist
                DummyDataService.RolePermissions.Add(new RolePermissionModel
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    IsGranted = isGranted
                });
            }

            TempData["SuccessMessage"] = "Permission updated successfully!";
            return RedirectToAction("Index");
        }
    }
}
