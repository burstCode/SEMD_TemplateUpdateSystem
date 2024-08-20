using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

using TemplatesWebsite.Services;
using TemplatesWebsite.Models;
using TemplatesWebsite.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.EnsureCreated())
    {
        // Если база данных только была создана, то добавляется стандартный аккаунт
        dbContext.Users.Add(
            new User {
                User_Email = "sales@simplex48.com",
                User_Name = "Симплекс", 
                User_Password = "de576dc430fd5767a0ce8b9122c095220ca413da542d620f896f3c2757230257aadc75c8b28e3744077586c70ab2d035db1a77bfa55dbc8042b97382f3fbe64d"
            });

        dbContext.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "templates",
    pattern: "{controller=Templates}/{action=Manage}/");

app.MapControllerRoute(
    name: "templates",
    pattern: "{controller=Account}/{action=Login}/");

app.Run();
