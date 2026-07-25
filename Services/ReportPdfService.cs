using QuestPDF.Fluent;
using the_fitness_assistant.Models;
using the_fitness_assistant.Services;
using the_fitness_assistant.Data;
using the_fitness_assistant.Documents;

namespace the_fitness_assistant.Services;

public class ReportPdfService
{
    public byte[] GeneratePdf(DashboardReport report)
    {
        var document = new ReportDocument(report);
        return document.GeneratePdf();
    }
}