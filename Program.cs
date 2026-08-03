using Microsoft.EntityFrameworkCore;
using the_fitness_assistant.Data;
using the_fitness_assistant.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using the_fitness_assistant.Services;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.DataProtection.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// builder.WebHost.UseUrls("http://0.0.0.0:5011");

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
    .SetApplicationName("the-fitness-assistant");

// Authentication google oauth
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add User Service that checks if email exists in users table in db for the registered/logged in user
builder.Services.AddScoped<UserService>();

// This Seeder service auto fills the users account with data for demo purposes
builder.Services.AddScoped<DemoDataSeeder>();

// This helps create the PDF for the dashboard report generator
builder.Services.AddScoped<ReportPdfService>();

// This feature stores motivational quotes on fitness and chooses one at random to display on the home page.
builder.Services.AddSingleton<QuoteService>();

// This feature allows the user to create daily tasks and tracks them and allows the LED display to return a result on the status of the tasks, meaning Green for all tasks done, Yellow for some tasks completed and red for no tasks completed.
builder.Services.AddScoped<DailyTaskService>();

// This service provides a API key for my pi device so that it can access the users task status for the LED circuit hardware.
builder.Services.AddScoped<DeviceApiKeyService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Services
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";

    options.Cookie.Name = "FitnessAssistant.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
    .AddGoogle(options =>
    {
        options.ClientId =
            builder.Configuration["Authentication:Google:ClientId"]!;

        options.ClientSecret =
            builder.Configuration["Authentication:Google:ClientSecret"]!;

        options.CallbackPath = "/signin-google";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseForwardedHeaders();

// app.UseHttpsRedirection();

app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapGet("/login", (string? returnUrl) =>
{
    Console.WriteLine("LOGIN ENDPOINT HIT");

    return Results.Challenge(
        new AuthenticationProperties
        {
            RedirectUri = "/auth-success"
        },
        new[] { GoogleDefaults.AuthenticationScheme });
});

app.MapPost("/logout", async (HttpContext context) =>
{
    Console.WriteLine("LOGOUT ENDPOINT HIT");
    await context.SignOutAsync();
    return Results.Redirect("/");
});

app.MapGet("/auth-success", async (
    HttpContext context,
    UserService userService,
    DemoDataSeeder demoDataSeeder) =>
{
    Console.WriteLine("AUTH-SUCCESS ENDPOINT HIT");

    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
        return Results.Redirect("/login");
    }

    var user = await userService.EnsureUserExistsAsync(context.User);

    await demoDataSeeder.SeedForUserAsync(user);

    return Results.Redirect("/");
});

app.MapGet("/api/status/{userId}", async (
    int userId,
    HttpContext context,
    DeviceApiKeyService apiKeyService,
    DailyTaskService dailyTaskService) =>
{
    var apiKey =
        context.Request.Headers["X-API-Key"].FirstOrDefault();

    if (!apiKeyService.IsValid(apiKey))
    {
        return Results.Unauthorized();
    }

    var status =
        await dailyTaskService.GetProgressStatusAsync(userId);

    return Results.Ok(status);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();



// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     DbSeeder.Seed(db);
// }

Console.WriteLine(builder.Configuration["Authentication:Google:ClientId"]);
Console.WriteLine(builder.Configuration["Authentication:Google:ClientSecret"]?.Length);

app.Run();
