using System.Net.Http.Headers;
using System.Text;
using Coltium_Test.Data;
using Coltium_Test.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddHttpClient("Mailgun", (serviceProvider, client) =>
{
    // Resolve dependencies
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    // Retrieve Mailgun configuration based on environment
    var apiKey = configuration["Mailgun:ApiKey"];
    var domain = configuration["Mailgun:Domain"];

    // Log warnings if required variables are missing
    if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(domain))
        logger.LogWarning("Mailgun API key or domain is not set. Ensure these values are configured properly.");

    // Set default HttpClient configuration
    client.BaseAddress = new Uri($"https://api.mailgun.net/v3/{domain}/messages");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Basic",
        Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"))
    );
});

// Register the email service in the DI container
builder.Services.AddTransient<IEmailSender, AzureSmtpEmailSenderService>();

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
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();