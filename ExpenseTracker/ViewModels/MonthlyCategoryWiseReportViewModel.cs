using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTracker.Services;
using Microcharts;
using SkiaSharp;

namespace ExpenseTracker.ViewModels
{
    public partial class MonthlyCategoryWiseReportViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private Chart chart;

        public MonthlyCategoryWiseReportViewModel(DatabaseService db)
        {
            _db = db;
        }

        public async Task LoadCategoryBarChartAsync()
        {
            var transactions = await _db.GetTransactionsAsync();

            var grouped = transactions
                .GroupBy(t => t.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(t => t.Type == "Income" ? t.Amount : -t.Amount)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            var entries = new List<ChartEntry>();

            foreach (var item in grouped)
            {
                entries.Add(new ChartEntry((float)item.Total)
                {
                    Label = item.Category,
                    ValueLabel = $"৳ {item.Total:N0}",
                    Color = item.Total >= 0 ? SKColor.Parse("#4caf50") : SKColor.Parse("#f44336")
                });
            }

            Chart = new BarChart
            {
                Entries = entries,
                LabelTextSize = 18,
                Margin = 10,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                MaxValue = grouped.Any() ? (float)(Convert.ToDouble(grouped.Max(x => x.Total)) * 1.1) : 0,
                MinValue = grouped.Any() ? (float)(Convert.ToDouble(grouped.Min(x => x.Total)) * 1.1) : 0
            };
        }
        
        public async Task LoadMonthlyCategoryPieChartAsync()
        {
            var transactions = await _db.GetTransactionsAsync();

            var now = DateTime.Now;
            var monthlyTransactions = transactions
                .Where(t => t.Date.Month == now.Month && t.Date.Year == now.Year)
                .ToList();

            var categories = _db.GetCategoryAsync().Result
                .Select(c => c.CategoryName)
                .Distinct()
                .ToList();

            int totalCategories = categories.Count;

            var categoryColorMap = categories
                .Select((categoryName, index) => new
                {
                    CategoryName = categoryName,
                    Color = GenerateColorFromIndex(index, totalCategories)
                })
                .ToDictionary(x => x.CategoryName, x => x.Color);

                //var categoryColors = new Dictionary<string, string>
                //{
                //    { "Salary", "#4caf50" },         // Green
                //    { "Market Cost", "#ff9800" },    // Orange
                //    { "Medicine", "#9c27b0" },       // Purple
                //    { "Bus Rent", "#2196f3" },       // Blue
                //    { "Loan EMI", "#f44336" },       // Red
                //    { "Breakfast", "#ff5722" },      // Deep Orange
                //    { "Snack", "#795548" },          // Brown
                //    { "Cigarette", "#607d8b" },      // Blue Grey
                //    { "Recharge", "#3f51b5" },       // Indigo
                //    { "Case Cost", "#e91e63" },      // Pink
                //    { "Others", "#009688" }          // Teal
                //};

            var grouped = categories
                .Select(cat => new
                {
                    Category = cat,
                    Total = monthlyTransactions
                        .Where(t => t.Category == cat)
                        .Sum(t => t.Type == "Income" ? t.Amount : -t.Amount)
                })
                .Where(x => x.Total != 0)
                .ToList();

            var entries = new List<ChartEntry>();

            foreach (var item in grouped)
            {
                var colorHex = categoryColorMap.ContainsKey(item.Category) ? categoryColorMap[item.Category] : "#cccccc";

                entries.Add(new ChartEntry((float)Math.Abs(item.Total))
                {
                    Label = item.Category,
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

        // Generate a color based on the index and total number of categories
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
