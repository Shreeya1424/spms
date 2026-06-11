using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using spms.Models;
using spms.Services;
using System.Security.Claims;

namespace spms.Controllers
{
    
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            // If already logged in, redirect to dashboard
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // ===== STEP 1: Find user in static list by email =====
            var user = DummyDataService.Users.FirstOrDefault(u =>
                u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == model.Password);

            // ===== STEP 2: Validate =====
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            if (user.Status != "Active")
            {
                ModelState.AddModelError(string.Empty, "Your account is inactive. Please contact the administrator.");
                return View(model);
            }

            // ===== STEP 3: Create Claims (stored inside the cookie) =====
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // User ID
                new Claim(ClaimTypes.Name, user.FullName),                  // Display Name
                new Claim(ClaimTypes.Email, user.Email),                    // Email
                new Claim(ClaimTypes.Role, user.Role)                       // Role (Admin/Faculty/Student)
            };

            // ===== STEP 4: Create the cookie =====
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,  // Remember Me checkbox
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)   // 7 days if "Remember Me"
                    : DateTimeOffset.UtcNow.AddHours(8)   // 8 hours otherwise
            };

            // ===== STEP 5: Sign in — this creates the cookie in the browser =====
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            TempData["SuccessMessage"] = $"Welcome back, {user.FullName}!";

            // Redirect to return URL or dashboard
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
