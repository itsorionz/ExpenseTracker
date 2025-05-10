using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Services;
using System.Collections.ObjectModel;
using System.Transactions;

namespace ExpenseTracker.ViewModels
{
    public partial class ExpenseAssumptionViewModel :  ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private decimal assumptionPerDay;
        [ObservableProperty]
        private decimal totalBalance;
        [ObservableProperty]
        private decimal totalIncome;
        [ObservableProperty]
        private decimal totalExpense;
        [ObservableProperty]
        private int remainingDays;

        public ExpenseAssumptionViewModel(DatabaseService db)
        {
           _db = db;
        }

        [RelayCommand]
        public void LoadExpenseAssumption()
        {
            var data = _db.GetTransactionsAsync().GetAwaiter().GetResult()
                        .Where(d => d.Date.Month == DateTime.Now.Date.Month);
            TotalIncome = data.Where(t => t.Type == "Income").Sum(t => t.Amount);
            TotalExpense = data.Where(t => t.Type == "Expense").Sum(t => t.Amount);
            TotalBalance = TotalIncome - TotalExpense;
            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            RemainingDays = daysInMonth - today.Day;
            AssumptionPerDay = RemainingDays > 0 ? TotalBalance / RemainingDays : 0;
        }
    }
}
