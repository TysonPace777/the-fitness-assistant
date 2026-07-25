using the_fitness_assistant.Models;

namespace the_fitness_assistant.Models;

public class DashboardReport
{
    public User User { get; set; } = new();

    public List<FoodLogEntry> RecentFoodLogs { get; set; } = new();

    public CalorieGoal? CalorieGoal { get; set; }
}