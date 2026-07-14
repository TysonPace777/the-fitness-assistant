using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using the_fitness_assistant.Data;
using the_fitness_assistant.Models;

namespace the_fitness_assistant.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> EnsureUserExistsAsync(ClaimsPrincipal principal)
    {
        Console.WriteLine("EnsureUserExistsAsync STARTED");

        var email = principal.FindFirst(ClaimTypes.Email)?.Value;

        Console.WriteLine($"Email found: {email}");

        if (string.IsNullOrEmpty(email))
        {
            throw new Exception("Authenticated user has no email claim.");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        Console.WriteLine(
            $"Existing user found: {user != null}"
        );

        if (user != null)
        {
            return user;
        }

        user = new User
        {
            Email = email,
            DisplayName = principal.Identity?.Name ?? email
        };

        _context.Users.Add(user);

        Console.WriteLine("Saving new user...");
        
        await _context.SaveChangesAsync();

        Console.WriteLine("User saved!");

        return user;
    }
}