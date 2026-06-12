using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("SPMS Application Starting - Version Premium UI v2");
builder.Services.AddControllersWithViews();

// ===== Cookie Authentication Configuration =====
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";           // Redirect here if not logged in
        options.LogoutPath = "/Account/Logout";         // Logout endpoint
        options.AccessDeniedPath = "/Account/AccessDenied"; // If user doesn't have permission
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // Cookie expires after 8 hours
        options.SlidingExpiration = true;               // Reset timer on activity
        options.Cookie.HttpOnly = true;                 // Prevent JS access to cookie
        options.Cookie.Name = "SPMS.Auth";              // Cookie name
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

// Forward headers from Render's reverse proxy
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// ===== Authentication MUST come before Authorization =====
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
