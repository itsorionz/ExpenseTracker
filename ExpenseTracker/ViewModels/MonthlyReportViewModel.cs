using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTracker.Services;
using Microcharts;
using SkiaSharp;
using System.Globalization;

namespace ExpenseTracker.ViewModels
{
    public partial class MonthlyReportViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private Chart chart;

        public MonthlyReportViewModel(DatabaseService db)
        {
            _db = db;
        }

        public async Task LoadMonthlyReportAsync()
        {
            var transactions = await _db.GetTransactionsAsync();

            var grouped = transactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(g => new
                {
                    Month = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month)} {g.Key.Year}",
                    Income = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expense = g.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            var entries = new List<ChartEntry>();

            foreach (var month in grouped)
            {
                entries.Add(new ChartEntry((float)month.Income)
                {
                    Label = month.Month,
                    ValueLabel = $"৳ {month.Income:N0}",
                    Color = SKColor.Parse("#4caf50")
                });

                entries.Add(new ChartEntry((float)month.Expense)
                {
                    Label = month.Month,
                    ValueLabel = $"-৳ {month.Expense:N0}",
                    Color = SKColor.Parse("#f44336")
                });
            }

            Chart = new Microcharts.BarChart
            {
                Entries = entries,
                LabelTextSize = 24,
                Margin = 10
            };

        }

        public async Task LoadDailyChartAsync()
        {
            var transactions = await _db.GetTransactionsAsync();
            var grouped = transactions
                .GroupBy(t => t.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Sum(t => t.Type == "Income" ? t.Amount : -t.Amount)
                })
                .OrderBy(x => x.Date)
                .ToList();

            var entries = new List<ChartEntry>();

            foreach (var item in grouped)
            {
                entries.Add(new ChartEntry((float)item.Total)
                {
                    Label = item.Date.ToString("dd MMM"),
                    ValueLabel = $"৳ {item.Total:N0}",
                    Color = item.Total >= 0 ? SKColor.Parse("#4caf50") : SKColor.Parse("#f44336")
                });
            }

            Chart = new BarChart
            {
                Entries = entries,
                LabelTextSize = 20,
                Margin = 10,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                MaxValue = grouped.Any() ? (float)(Convert.ToDouble(grouped.Max(x => x.Total)) * 1.1) : 0,
                MinValue = grouped.Any() ? (float)(Convert.ToDouble(grouped.Min(x => x.Total)) * 1.1) : 0
            };
        }
    }
}
