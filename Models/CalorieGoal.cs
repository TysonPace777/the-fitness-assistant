using System.ComponentModel.DataAnnotations;
namespace the_fitness_assistant.Models;

public class CalorieGoal
{
    public int CalorieGoalId { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string GoalType { get; set; } = "";

    public int DailyCalories { get; set; }
}