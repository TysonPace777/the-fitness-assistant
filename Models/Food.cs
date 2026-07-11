using System.ComponentModel.DataAnnotations;
namespace the_fitness_assistant.Models;

public class Food
{
    public int FoodId { get; set; }

    public string Name { get; set; } = "";

    public int Calories { get; set; }

    public string ServingSize { get; set; } = "";

    public int? CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }

    public ICollection<FoodLogEntry> FoodLogEntries { get; set; } = new List<FoodLogEntry>();
}