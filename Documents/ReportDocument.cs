using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using the_fitness_assistant.Models;

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
            });

            page.Content().Column(col =>
            {
                col.Spacing(8);

                col.Item().Text("Profile")
                    .FontSize(18)
                    .Bold();

                col.Item().Text($"Height: {_report.User.Height}");
                col.Item().Text($"Weight: {_report.User.Weight}");
                col.Item().Text($"Age: {_report.User.Age}");

                if (_report.CalorieGoal != null)
                {
                    col.Item().Text($"Calorie Goal: {_report.CalorieGoal.DailyCalories} cal/day");
                    col.Item().Text($"Goal: {_report.CalorieGoal.GoalType}");
                }

                col.Item().PaddingVertical(15);

                col.Item().Text("Recent Food Logs")
                    .FontSize(18)
                    .Bold();

                foreach (var log in _report.RecentFoodLogs)
                {
                    col.Item().BorderBottom(1).PaddingBottom(5).Column(food =>
                    {
                        food.Item().Text(log.LogDate.ToShortDateString())
                            .Bold();

                        food.Item().Text(log.MealType);

                        food.Item().Text(log.Food.Name);

                        food.Item().Text($"{log.Food.Calories} cal");
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