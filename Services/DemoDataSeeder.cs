using Microsoft.EntityFrameworkCore;
using the_fitness_assistant.Data;
using the_fitness_assistant.Models;

namespace the_fitness_assistant.Services;

public class DemoDataSeeder
{
    private readonly ApplicationDbContext _context;

    public DemoDataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task SeedForUserAsync(User user)
    {
        Console.WriteLine("Checking demo data...");


        var hasFood =
            await _context.Foods.AnyAsync(f => f.CreatedByUserId == user.UserId);

        if (hasFood)
        {
            Console.WriteLine("Demo data already exists.");
            return;
        }


        Console.WriteLine("Creating demo data...");

        // First login demo profile setup
        // Only runs when demo food data does not already exist
        user.Height = 64; // inches
        user.Weight = 165; // pounds
        user.Age = 35;

        _context.Users.Update(user);

        await _context.SaveChangesAsync();


        var foods = new List<Food>
        {
            new Food
            {
                Name = "Chicken Breast",
                Calories = 280,
                ServingSize = "6 oz",
                CreatedByUserId = user.UserId
            },

            new Food
            {
                Name = "Brown Rice",
                Calories = 215,
                ServingSize = "1 cup",
                CreatedByUserId = user.UserId
            },

            new Food
            {
                Name = "Apple",
                Calories = 95,
                ServingSize = "1 medium",
                CreatedByUserId = user.UserId
            }
        };


        _context.Foods.AddRange(foods);

        await _context.SaveChangesAsync();


        var calorieGoal = new CalorieGoal
        {
            UserId = user.UserId,
            GoalType = "Weight Loss",
            DailyCalories = 1800
        };


        _context.CalorieGoals.Add(calorieGoal);


        await _context.SaveChangesAsync();


        var logs = new List<FoodLogEntry>
        {
            new FoodLogEntry
            {
                UserId = user.UserId,
                FoodId = foods[0].FoodId,
                LogDate = DateTime.UtcNow,
                MealType = "Dinner",
                Servings = 1
            },

            new FoodLogEntry
            {
                UserId = user.UserId,
                FoodId = foods[1].FoodId,
                LogDate = DateTime.UtcNow,
                MealType = "Lunch",
                Servings = 1
            },

            new FoodLogEntry
            {
                UserId = user.UserId,
                FoodId = foods[2].FoodId,
                LogDate = DateTime.UtcNow,
                MealType = "Snack",
                Servings = 1
            }
        };


        _context.FoodLogEntries.AddRange(logs);

        await _context.SaveChangesAsync();


        Console.WriteLine("Demo data created.");
    }
}