using ExpenseTracker.Models;
using QuestPDF.Fluent;
using QContainer = QuestPDF.Infrastructure.IContainer;
using Colors = QuestPDF.Helpers.Colors;

namespace ExpenseTracker.Services
{
    public class PdfService
    {
        public Task<byte[]> CreatePdfAsync(List<Transaction> transactions)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(c => CellStyle(c)).Text("Date").Bold();
                                header.Cell().Element(c => CellStyle(c)).Text("Category").Bold();
                                header.Cell().Element(c => CellStyle(c)).Text("Amount").Bold();
                            });

                            // Body
                            foreach (var t in transactions)
                            {
                                table.Cell().Element(c => CellStyle(c)).Text(t.Date.ToString("yyyy-MM-dd"));
                                table.Cell().Element(c => CellStyle(c)).Text(t.Category);
                                table.Cell().Element(c => CellStyle(c)).Text($"৳ {t.Amount:N2}");
                            }
                        });
                });
            });

            return Task.FromResult(document.GeneratePdf());
        }

        private QContainer CellStyle(QContainer container)
        {
            return container
                .PaddingVertical(5)
                .PaddingHorizontal(10)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .AlignLeft()
                .AlignMiddle();
        }




    }

}
