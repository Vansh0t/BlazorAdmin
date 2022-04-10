using BlazorAdmin.Examples.Areas.Identity;
using BlazorAdmin.Examples.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using BlazorAdmin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbContext, ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddBlazorAdmin("Admin");

builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var ctx = services.GetRequiredService<ApplicationDbContext>();
    var uMgr = services.GetRequiredService<UserManager<AppUser>>();
    var rMgr = services.GetRequiredService<RoleManager<AppRole>>();
    ctx.Database.EnsureCreated();
    var r1 = rMgr.CreateAsync(new AppRole("Admin")).Result;
    var user = uMgr.FindByNameAsync("user@example.com").Result;
    if(user is null)
    {
        user = new AppUser { UserName = "user@example.com", Email = "user@example.com" };
        var r2 = uMgr.CreateAsync(user, "Pwd_111").Result;
    }
    var r3 = uMgr.AddToRoleAsync(user, "Admin").Result;
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();