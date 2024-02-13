using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
using Sportle.Web.Extensions;
using Sportle.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<StringService>();
builder.Services.AddScoped<ResultsService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<SportleDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SportleDbContext>();

builder.Services.AddAuthentication()
    .TryAddGoogle(builder.Configuration.GetSection("Authentication:Google"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<SportleDbContext>();
dbContext.Repair(true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
