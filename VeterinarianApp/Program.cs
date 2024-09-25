using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VeterinarianApp.Data;
using VeterinarianApp.Helpers;
using VeterinarianApp.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Identity services with custom user class
builder.Services.AddDefaultIdentity<AdminUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Add roles support
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddScoped<IPasswordHasher<Veterinarian>, PasswordHasher<Veterinarian>>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "YourAppName.Cookie";
    //options.LoginPath = "/Account/Login";
    //options.LogoutPath = "/Account/Logout";
    //options.AccessDeniedPath = "/Account/AccessDenied";
});
// Load configuration from appsettings.json

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));



var app = builder.Build();

// Apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Apply any pending migrations
    dbContext.Database.Migrate();

    var userManager = services.GetRequiredService<UserManager<AdminUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Initialize the database and seed the admin user
    await SeedDataAsync(userManager, roleManager);
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
//app.Use(async (context, next) =>
//{
//    if (!context.User.Identity.IsAuthenticated && context.Request.Path != "/AdminLogin")
//    {
//        context.Response.Redirect("/AdminLogin");
//        return;
//    }

//    await next();
//});

app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/Index")); // Default route


app.Run();


static async Task SeedDataAsync(UserManager<AdminUser> userManager, RoleManager<IdentityRole> roleManager)
{
    string adminRole = "Admin";
    const string adminEmail = "admin@gmail.com";
    const string adminPassword = "Admin@1234";
    const string firstName = "Admin";
    const string lastName = "User";

    // Create Admin role if it does not exist
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    // Check if the admin user already exists
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new AdminUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = firstName,
            LastName = lastName,
        };
        //await userManager.CreateAsync(adminUser, adminPassword);
        //await userManager.AddToRoleAsync(adminUser, adminRole);
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            // Assign the "Admin" role to the admin user
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}