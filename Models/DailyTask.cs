using System.ComponentModel.DataAnnotations;

namespace the_fitness_assistant.Models;

public class DailyTask
{
    public int DailyTaskId { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = "";

    [StringLength(500)]
    public string Description { get; set; } = "";

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<DailyTaskCompletion> Completions { get; set; }
        = new List<DailyTaskCompletion>();
}