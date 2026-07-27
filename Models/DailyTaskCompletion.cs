namespace the_fitness_assistant.Models;

public class DailyTaskCompletion
{
    public int DailyTaskCompletionId { get; set; }

    public int DailyTaskId { get; set; }

    public DailyTask DailyTask { get; set; } = null!;

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public DateTime? CompletedAt { get; set; }

    public bool Completed { get; set; } = false;
}