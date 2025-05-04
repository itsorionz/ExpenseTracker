using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTracker.Services;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModels
{
    public partial class ReportItem
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
    }

    public class ReportViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        public ObservableCollection<ReportItem> ReportData { get; } = new();

        public ReportViewModel(DatabaseService db)
        {
            _db = db;
            LoadReport();
        }

        private async void LoadReport()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var items = await _db.GetTransactionsAsync();

            var grouped = items
                .Where(t => t.Type == "Expense" && t.Date.Month == month && t.Date.Year == year)
                .GroupBy(t => t.Category)
                .Select(g => new ReportItem { Category = g.Key, Amount = g.Sum(x => x.Amount) });

            ReportData.Clear();
            foreach (var item in grouped)
                ReportData.Add(item);
        }
    }

}
