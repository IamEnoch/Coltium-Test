using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Coltium_Test.Data;
using Coltium_Test.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Set up your named HttpClient for easy reuse
builder.Services.AddHttpClient("Mailgun", client =>
{
    // Grab values from the configuration
    var apiKey = builder.Configuration.GetValue<string>("Mailgun:ApiKey");
    var base64Auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"));
    var domain = builder.Configuration.GetValue<string>("Mailgun:Domain");

    // Set default values on the HttpClient
    client.BaseAddress = new Uri($"https://api.mailgun.net/v3/{domain}/messages");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);
});

builder.Services.AddTransient<IEmailSender, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();