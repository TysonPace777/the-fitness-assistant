namespace the_fitness_assistant.Models;

/// <summary>
/// One day's worth of logging, used by the weekly summary on the dashboard
/// and in the PDF report.
/// </summary>
public class DaySummary
{
    public DateTime Date { get; set; }

    public int Calories { get; set; }

    public int EntryCount { get; set; }

    /// <summary>
    /// Short weekday label, e.g. "Mon".
    /// </summary>
    public string DayLabel => Date.ToString("ddd");
}
