using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Staffly.BLL.Interfaces;
using Staffly.BLL.Repositories;
using Staffly.DAL.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<StafflyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<StafflyDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(
    config =>
    {
        config.LoginPath = "/Auth/SignIn";
        config.LogoutPath = "/Auth/SignOut";
        config.AccessDeniedPath = "/Auth/AccessDenied";
        config.SlidingExpiration = true;
        config.ExpireTimeSpan = TimeSpan.FromDays(30);
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
