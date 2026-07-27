public class DailyTaskItemViewModel
{
    public int CompletionId { get; set; }

    public int TaskId { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public bool Completed { get; set; }
}