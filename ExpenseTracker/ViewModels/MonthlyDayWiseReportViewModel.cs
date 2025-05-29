using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTracker.Services;
using Microcharts;
using SkiaSharp;
using System.Globalization;

namespace ExpenseTracker.ViewModels
{
    public partial class MonthlyDayWiseReportViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private Chart chart;

        public MonthlyDayWiseReportViewModel(DatabaseService db)
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
                    Label = item.Date.ToString("dd"),
                    ValueLabel = $"৳ {item.Total:N0}",
                    Color = item.Total >= 0 ? SKColor.Parse("#4caf50") : SKColor.Parse("#f44336")
                });
            }

            Chart = new BarChart
            {
                Entries = entries,
                LabelTextSize = 16,
                Margin = 20,
                ValueLabelOrientation = Orientation.Vertical,
                LabelOrientation = Orientation.Vertical,
                MaxValue = grouped.Any() ? (float)(Convert.ToDouble(grouped.Max(x => x.Total)) * 1.1) : 0,
                MinValue = grouped.Any() ? (float)(Convert.ToDouble(grouped.Min(x => x.Total)) * 1.1) : 0
            };

            //Chart = new LineChart
            //{
            //    Entries = entries,
            //    LabelTextSize = 30,
            //    LineMode = LineMode.Straight,
            //    LineSize = 8,
            //    PointMode = PointMode.Circle,
            //    PointSize = 10,
            //    Margin = 20,
            //    ValueLabelOrientation = Orientation.Horizontal,
            //    LabelOrientation = Orientation.Horizontal,
            //    MaxValue = (float)(Convert.ToDouble(entries.Max(e => e.Value)) * 1.1f),
            //    MinValue = (float)(Convert.ToDouble(entries.Min(e => e.Value)) * 1.1f)
            //};
        }

        public async Task LoadMonthlyDateWisePieChartAsync()
        {
            var transactions = await _db.GetTransactionsAsync();

            var now = DateTime.Now;
            var monthlyTransactions = transactions
                .Where(t => t.Date.Month == now.Month && t.Date.Year == now.Year)
                .ToList();

            var dates = transactions
                        .Where(d => d.Date.Month == now.Month && d.Date.Year == now.Year)
                        .Select(d => d.Date.Date).Distinct().ToList();

            int totalDates = dates.Count;

            var dateColorMap = dates
                .Select((Date, index) => new
                {
                    Date = Date,
                    Color = GenerateColorFromIndex(index, totalDates)
                })
                .ToDictionary(x => x.Date, x => x.Color);


            var grouped = dates
                .Select(dat => new
                {
                    Date = dat,
                    Total = monthlyTransactions
                        .Where(t => t.Date == dat)
                        .Sum(t => t.Type == "Income" ? t.Amount : -t.Amount)
                })
                .Where(x => x.Total != 0)
                .ToList();

            var entries = new List<ChartEntry>();

            foreach (var item in grouped)
            {
                var colorHex = dateColorMap.ContainsKey(item.Date) ? dateColorMap[item.Date] : "#cccccc";

                entries.Add(new ChartEntry((float)Math.Abs(item.Total))
                {
                    Label = item.Date.ToString("dd-MM-yyyy"),
                    ValueLabel = $"৳ {item.Total:N0}",
                    Color = SKColor.Parse(colorHex),
                    TextColor = SKColors.Black
                });
            }

            Chart = new PieChart
            {
                Entries = entries,
                LabelTextSize = 20,
                BackgroundColor = SKColors.Transparent,
                HoleRadius = 0.3f
            };
        }


        public string GenerateColorFromIndex(int index, int total)
        {
            double hue = (360.0 / total) * index;
            return HslToHex(hue, 0.6, 0.6);
        }

        public string HslToHex(double h, double s, double l)
        {
            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = l - c / 2;
            double r = 0, g = 0, b = 0;

            if (h < 60) { r = c; g = x; b = 0; }
            else if (h < 120) { r = x; g = c; b = 0; }
            else if (h < 180) { r = 0; g = c; b = x; }
            else if (h < 240) { r = 0; g = x; b = c; }
            else if (h < 300) { r = x; g = 0; b = c; }
            else { r = c; g = 0; b = x; }

            int r255 = (int)((r + m) * 255);
            int g255 = (int)((g + m) * 255);
            int b255 = (int)((b + m) * 255);

            return $"#{r255:X2}{g255:X2}{b255:X2}";
        }
    }
}
