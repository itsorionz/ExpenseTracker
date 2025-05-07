using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class PdfService
    {
        public async Task<string> CreatePdfAsync(List<Transaction> transactions)
        {
            // Create a new PDF document
            using var document = new PdfDocument();
            var page = document.Pages.Add();

            // Define fonts and brushes
            var font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            var boldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            var brush = PdfBrushes.Black;

            float y = 10;

            // Headers
            page.Graphics.DrawString("Date", boldFont, brush, new Syncfusion.Drawing.PointF(0, y));
            page.Graphics.DrawString("Category", boldFont, brush, new Syncfusion.Drawing.PointF(100, y));
            page.Graphics.DrawString("Amount", boldFont, brush, new Syncfusion.Drawing.PointF(250, y));
            y += 20;

            // Data rows
            foreach (var t in transactions)
            {
                page.Graphics.DrawString(t.Date.ToString("yyyy-MM-dd"), font, brush, new Syncfusion.Drawing.PointF(0, y));
                page.Graphics.DrawString(t.Category, font, brush, new Syncfusion.Drawing.PointF(100, y));
                page.Graphics.DrawString($"৳ {t.Amount:N2}", font, brush, new Syncfusion.Drawing.PointF(250, y));
                y += 20;
            }

            // Save to memory stream
            using var stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            // Save to file
            var fileName = $"Transaction_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using var fileStream = File.Create(filePath);
            stream.CopyTo(fileStream);

            return filePath; // return path to the generated file
        }
    }
}
