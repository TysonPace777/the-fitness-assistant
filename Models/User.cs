using System.ComponentModel.DataAnnotations;
namespace SunsetSchedule.Models;

public class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = "";

    public string DisplayName { get; set; } = "";

    public double Height { get; set; }

    public double Weight { get; set; }

    public int Age { get; set; }

    public ICollection<FoodLogEntry> FoodLogEntries { get; set; } = new List<FoodLogEntry>();

    public ICollection<Food> Foods { get; set; } = new List<Food>();

    public ICollection<CalorieGoal> CalorieGoals { get; set; } = new List<CalorieGoal>();
}
