using Microsoft.EntityFrameworkCore;
using the_fitness_assistant.Data;
using the_fitness_assistant.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using the_fitness_assistant.Services;

var builder = WebApplication.CreateBuilder(args);

// Authentication google oauth
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add User Service that checks if email exists in users table in db for the registered/logged in user
builder.Services.AddScoped<UserService>();

// This Seeder service auto fills the users account with data for demo purposes
builder.Services.AddScoped<DemoDataSeeder>();

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
    })
    .AddGoogle(options =>
    {
        options.ClientId =
            builder.Configuration["Authentication:Google:ClientId"]!;

        options.ClientSecret =
            builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapGet("/login", (string? returnUrl) =>
{
    Console.WriteLine("LOGIN ENDPOINT HIT");

    return Results.Challenge(
        new AuthenticationProperties { RedirectUri = "/auth-success" },
        new[] { GoogleDefaults.AuthenticationScheme });
});

app.MapPost("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
});

// This is where Login redirects so that after authentication/registration success a user entry in the users table is automatically created.  This also auto runs the database seeder when it detects a new user has been added.
app.MapGet("/auth-success", async (
    HttpContext context,
    UserService userService,
    DemoDataSeeder demoDataSeeder) =>
{
    Console.WriteLine("AUTH-SUCCESS ENDPOINT HIT");

    var user = await userService.EnsureUserExistsAsync(
        context.User
    );

    await demoDataSeeder.SeedForUserAsync(user);

    return Results.Redirect("/");
});

// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     DbSeeder.Seed(db);
// }

app.Run();
