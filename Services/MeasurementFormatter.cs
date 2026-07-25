namespace the_fitness_assistant.Services;

/// <summary>
/// Formats the raw profile numbers for display.
/// Shared so the dashboard and the PDF report always agree.
/// </summary>
public static class MeasurementFormatter
{
    /// <summary>
    /// Height is stored as total inches. Shown as feet and inches, e.g. 5' 9".
    /// </summary>
    public static string FormatHeight(double totalInches)
    {
        if (totalInches <= 0)
        {
            return "Not set";
        }

        int feet = (int)(totalInches / 12);
        int inches = (int)Math.Round(totalInches % 12);

        // Rounding 11.6 inches up gives 12, which belongs in the feet column.
        if (inches == 12)
        {
            feet = feet + 1;
            inches = 0;
        }

        return $"{feet}' {inches}\"";
    }

    /// <summary>
    /// Weight is stored in pounds.
    /// </summary>
    public static string FormatWeight(double pounds)
    {
        return pounds > 0 ? $"{pounds:F0} lbs" : "Not set";
    }

    public static string FormatAge(int years)
    {
        return years > 0 ? $"{years} yrs" : "Not set";
    }
}
