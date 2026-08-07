using System.ComponentModel.DataAnnotations;
namespace the_fitness_assistant.Models;

public class Food
{
    public int FoodId { get; set; }

    [Required(ErrorMessage = "Please enter a food name.")]
    [StringLength(100, ErrorMessage = "Food name cannot be more than 100 characters.")]
    public string Name { get; set; } = "";

    [Range(1, 10000, ErrorMessage = "Calories must be between 1 and 10000.")]
    public int Calories { get; set; }

    [Required(ErrorMessage = "Please enter a serving size, for example 1 cup or 100g.")]
    [StringLength(50, ErrorMessage = "Serving size cannot be more than 50 characters.")]
    public string ServingSize { get; set; } = "";

    public int? CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }

    public ICollection<FoodLogEntry> FoodLogEntries { get; set; } = new List<FoodLogEntry>();
}