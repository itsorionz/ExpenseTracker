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
        private Microcharts.BarChart chart;
        [ObservableProperty]
        private Microcharts.LineChart chart2;

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

            //Chart = new Microcharts.BarChart 
            //{
            //    Entries = entries,
            //    LabelTextSize = 24,
            //    Margin = 10
            //};

            Chart2 = new LineChart
            {
                Entries = entries,
                LabelTextSize = 24,
                Margin = 10
            };
        }
    }
}
