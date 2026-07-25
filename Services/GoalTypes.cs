namespace the_fitness_assistant.Services;

/// <summary>
/// One place for the values stored in CalorieGoal.GoalType.
/// Use these constants instead of typing the strings by hand, so the
/// seeder, the calculator and the reports can never drift apart.
/// </summary>
public static class GoalTypes
{
    public const string Lose = "Lose";

    public const string Maintain = "Maintain";

    public const string Gain = "Gain";

    /// <summary>
    /// Turns a stored value into the wording shown to the user.
    /// Older rows were seeded as "Weight Loss", so those are mapped too.
    /// </summary>
    public static string ToDisplayName(string? goalType)
    {
        return goalType switch
        {
            Lose => "Weight Loss",
            Maintain => "Maintenance",
            Gain => "Weight Gain",

            // Values written before the goal types were standardised.
            "Weight Loss" => "Weight Loss",
            "Weight Gain" => "Weight Gain",

            null => "Not set",
            "" => "Not set",

            _ => goalType
        };
    }
}
