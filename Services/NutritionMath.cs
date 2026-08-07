using the_fitness_assistant.Models;

namespace the_fitness_assistant.Services;

/// <summary>
/// One place for calorie arithmetic.
///
/// FoodLogEntry.Servings existed in the model but nothing ever read it, so a
/// log of "3 servings of rice" counted as one serving everywhere. Every page,
/// and the PDF report, now go through here so they can't drift apart.
/// </summary>
public static class NutritionMath
{
    /// <summary>
    /// Calories for a single log entry, taking the serving count into account.
    /// Older rows were written before Servings was populated, so a missing or
    /// zero value is treated as one serving rather than as zero calories.
    /// </summary>
    public static int CaloriesFor(FoodLogEntry? entry)
    {
        if (entry?.Food == null)
        {
            return 0;
        }

        double servings = entry.Servings <= 0 ? 1 : entry.Servings;

        return (int)Math.Round(entry.Food.Calories * servings);
    }

    /// <summary>
    /// Total calories across any set of log entries.
    /// </summary>
    public static int TotalCalories(IEnumerable<FoodLogEntry>? entries)
    {
        if (entries == null)
        {
            return 0;
        }

        return entries.Sum(CaloriesFor);
    }

    /// <summary>
    /// Total calories for one meal ("Breakfast", "Lunch", ...).
    /// </summary>
    public static int CaloriesForMeal(
        IEnumerable<FoodLogEntry>? entries,
        string mealType)
    {
        if (entries == null)
        {
            return 0;
        }

        // Goes through MealTypes so the older singular "Snack" rows still
        // match a lookup for "Snacks".
        var forMeal = entries.Where(e => MealTypes.Matches(e.MealType, mealType));

        return TotalCalories(forMeal);
    }

    /// <summary>
    /// The calendar day an entry belongs to, in the viewer's local time.
    ///
    /// LogDate is stored as "timestamp with time zone" and comes back from
    /// Npgsql as UTC. Grouping on the raw UTC date would push anything logged
    /// late in the evening in a behind-UTC timezone onto the following day, so
    /// convert first.
    /// </summary>
    public static DateTime LocalDay(FoodLogEntry entry)
    {
        if (entry.LogDate.Kind == DateTimeKind.Utc)
        {
            return entry.LogDate.ToLocalTime().Date;
        }

        return entry.LogDate.Date;
    }

    /// <summary>
    /// Calories logged on one local calendar day.
    /// </summary>
    public static int CaloriesForDay(
        IEnumerable<FoodLogEntry>? entries,
        DateTime localDay)
    {
        if (entries == null)
        {
            return 0;
        }

        return TotalCalories(entries.Where(e => LocalDay(e) == localDay.Date));
    }

    /// <summary>
    /// How far through the daily goal the user is, capped at 100 so it can be
    /// used directly as a progress bar width. Returns 0 when no goal is set.
    /// </summary>
    public static int PercentOfGoal(int consumed, int? dailyGoal)
    {
        if (dailyGoal == null || dailyGoal <= 0)
        {
            return 0;
        }

        int percent = (int)Math.Round(consumed * 100.0 / dailyGoal.Value);

        return Math.Clamp(percent, 0, 100);
    }

    /// <summary>
    /// Calories left before the goal is hit. Negative means over the goal.
    /// </summary>
    public static int RemainingCalories(int consumed, int? dailyGoal)
    {
        if (dailyGoal == null || dailyGoal <= 0)
        {
            return 0;
        }

        return dailyGoal.Value - consumed;
    }

    /// <summary>
    /// "1 serving" / "1.5 servings" — avoids printing "1.0".
    /// </summary>
    public static string FormatServings(double servings)
    {
        double value = servings <= 0 ? 1 : servings;

        string number = value % 1 == 0
            ? value.ToString("F0")
            : value.ToString("0.##");

        return value == 1 ? $"{number} serving" : $"{number} servings";
    }
}
