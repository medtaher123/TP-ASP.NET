using TP7.Application.Services;
using TP7.Presentation.Controllers;
using TP7.Infrastructure.Persistence.Repositories;
using TP7.Infrastructure.Persistence.DBContexts;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<SQLiteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped( typeof(GenericRepository<>));

builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<GenreService>();
// Not done with generic repository
builder.Services.AddScoped<CustomerService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Razor to use custom view folder
builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{

    // Add custom locations for views
    options.ViewLocationFormats.Add("/Presentation/ViewModels/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Presentation/ViewModels/Shared/{0}.cshtml");
});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





app.Run();
