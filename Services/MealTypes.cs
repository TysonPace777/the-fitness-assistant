namespace the_fitness_assistant.Services;

/// <summary>
/// One place for the values stored in FoodLogEntry.MealType.
///
/// The seeder was writing "Snack" while the tracker page is labelled "Snacks",
/// so any per-meal total looked up by the tracker's label would have silently
/// skipped every seeded snack. Use these constants instead of typing the
/// strings by hand, and run anything read from the database through
/// <see cref="Normalise"/> so the older rows still match.
///
/// This mirrors <see cref="GoalTypes"/>, which solves the same problem for
/// CalorieGoal.GoalType.
/// </summary>
public static class MealTypes
{
    public const string Breakfast = "Breakfast";

    public const string Lunch = "Lunch";

    public const string Dinner = "Dinner";

    public const string Snacks = "Snacks";

    /// <summary>
    /// The meals in the order the tracker shows them.
    /// </summary>
    public static readonly string[] All =
    {
        Breakfast,
        Lunch,
        Dinner,
        Snacks
    };

    /// <summary>
    /// Maps whatever is in the database onto one of the canonical values.
    /// Comparison is case-insensitive, and the older singular "Snack" rows are
    /// mapped onto "Snacks". Anything unrecognised is returned unchanged so no
    /// data is hidden from the user.
    /// </summary>
    public static string Normalise(string? mealType)
    {
        if (string.IsNullOrWhiteSpace(mealType))
        {
            return "";
        }

        string value = mealType.Trim();

        foreach (var meal in All)
        {
            if (string.Equals(value, meal, StringComparison.OrdinalIgnoreCase))
            {
                return meal;
            }
        }

        // Written before the meal types were standardised.
        if (string.Equals(value, "Snack", StringComparison.OrdinalIgnoreCase))
        {
            return Snacks;
        }

        return value;
    }

    /// <summary>
    /// The wording shown to the user.
    /// </summary>
    public static string ToDisplayName(string? mealType)
    {
        string normalised = Normalise(mealType);

        return string.IsNullOrEmpty(normalised) ? "Unspecified" : normalised;
    }

    /// <summary>
    /// True when two stored values refer to the same meal.
    /// </summary>
    public static bool Matches(string? a, string? b)
    {
        return string.Equals(
            Normalise(a),
            Normalise(b),
            StringComparison.OrdinalIgnoreCase);
    }
}
