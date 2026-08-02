using the_fitness_assistant.Models;

namespace the_fitness_assistant.Models;

public class DashboardReport
{
    public User User { get; set; } = new();

    public List<FoodLogEntry> RecentFoodLogs { get; set; } = new();

    public CalorieGoal? CalorieGoal { get; set; }

    /// <summary>
    /// Calories logged today, already serving-adjusted.
    /// </summary>
    public int TodayCalories { get; set; }

    /// <summary>
    /// One entry per day for the last seven days, oldest first.
    /// </summary>
    public List<DaySummary> WeeklySummary { get; set; } = new();
}
