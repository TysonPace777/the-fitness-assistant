using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using the_fitness_assistant.Models;
using the_fitness_assistant.Services;

namespace the_fitness_assistant.Documents;

public class ReportDocument : IDocument
{
    private readonly DashboardReport _report;

    public ReportDocument(DashboardReport report)
    {
        _report = report;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);

            page.Header().Column(col =>
            {
                col.Item().Text("Fitness Dashboard")
                    .FontSize(24)
                    .Bold();

                col.Item().PaddingTop(10);

                col.Item().Text($"Welcome, {_report.User.DisplayName}!")
                    .FontSize(18)
                    .Bold();

                col.Item().Text($"Generated {DateTime.Now:dddd, d MMMM yyyy}")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken1);
            });

            page.Content().Column(col =>
            {
                col.Spacing(8);

                col.Item().Text("Profile")
                    .FontSize(18)
                    .Bold();

                col.Item().Text(
                    $"Height: {MeasurementFormatter.FormatHeight(_report.User.Height)}");

                col.Item().Text(
                    $"Weight: {MeasurementFormatter.FormatWeight(_report.User.Weight)}");

                col.Item().Text(
                    $"Age: {MeasurementFormatter.FormatAge(_report.User.Age)}");

                if (_report.CalorieGoal != null)
                {
                    col.Item().Text($"Calorie Goal: {_report.CalorieGoal.DailyCalories} cal/day");
                    col.Item().Text(
                        $"Goal: {GoalTypes.ToDisplayName(_report.CalorieGoal.GoalType)}");
                }
                else
                {
                    col.Item().Text("Calorie Goal: not set")
                        .FontColor(Colors.Grey.Darken1);
                }

                col.Item().PaddingVertical(10);

                // ---- Today against the goal ----

                col.Item().Text("Today")
                    .FontSize(18)
                    .Bold();

                col.Item().Text($"Logged today: {_report.TodayCalories} cal");

                if (_report.CalorieGoal != null)
                {
                    int remaining = NutritionMath.RemainingCalories(
                        _report.TodayCalories,
                        _report.CalorieGoal.DailyCalories);

                    if (remaining >= 0)
                    {
                        col.Item().Text($"Remaining: {remaining} cal");
                    }
                    else
                    {
                        col.Item()
                            .Text($"Over goal by {Math.Abs(remaining)} cal")
                            .FontColor(Colors.Red.Darken2);
                    }
                }

                col.Item().PaddingVertical(10);

                // ---- Last seven days ----

                col.Item().Text("Last 7 Days")
                    .FontSize(18)
                    .Bold();

                if (_report.WeeklySummary.Any(d => d.Calories > 0))
                {
                    foreach (var day in _report.WeeklySummary)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .Text($"{day.Date:ddd d MMM}");

                            row.ConstantItem(90)
                                .AlignRight()
                                .Text($"{day.Calories} cal");
                        });
                    }

                    var loggedDays = _report.WeeklySummary
                        .Where(d => d.Calories > 0)
                        .ToList();

                    int average = (int)Math.Round(loggedDays.Average(d => d.Calories));

                    col.Item()
                        .PaddingTop(4)
                        .Text($"Daily average: {average} cal over {loggedDays.Count} logged day(s)")
                        .FontColor(Colors.Grey.Darken2);
                }
                else
                {
                    col.Item()
                        .Text("Nothing logged in the last seven days.")
                        .FontColor(Colors.Grey.Darken1);
                }

                col.Item().PaddingVertical(10);

                // ---- Recent entries ----

                col.Item().Text("Recent Food Logs")
                    .FontSize(18)
                    .Bold();

                if (!_report.RecentFoodLogs.Any())
                {
                    col.Item()
                        .Text("No food logs yet.")
                        .FontColor(Colors.Grey.Darken1);
                }

                foreach (var log in _report.RecentFoodLogs)
                {
                    col.Item().BorderBottom(1).PaddingBottom(5).Column(food =>
                    {
                        food.Item().Text(log.LogDate.ToShortDateString())
                            .Bold();

                        food.Item().Text(MealTypes.ToDisplayName(log.MealType));

                        food.Item().Text(log.Food?.Name ?? "Unknown food");

                        // Servings are now reflected in the calorie figure so
                        // the PDF agrees with the dashboard.
                        food.Item().Text(
                            $"{NutritionMath.FormatServings(log.Servings)} " +
                            $"\u2014 {NutritionMath.CaloriesFor(log)} cal");
                    });
                }
            });

            page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
        });
    }
}
