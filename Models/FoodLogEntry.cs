using System.ComponentModel.DataAnnotations;
namespace the_fitness_assistant.Models;

public class FoodLogEntry
{
    public int FoodLogEntryId { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int FoodId { get; set; }

    public Food Food { get; set; } = null!;

    public DateTime LogDate { get; set; }

    public string MealType { get; set; } = "";

    public double Servings { get; set; }
}