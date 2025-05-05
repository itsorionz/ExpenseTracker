using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModels
{
    public partial class DailyTransactionViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        [ObservableProperty] private ObservableCollection<Transaction> filteredTransactions = new();

        public DailyTransactionViewModel(DatabaseService db)
        {
            _db = db;
        }

        [RelayCommand]
        public async Task LoadTransactions()
        {
            var all = await _db.GetTransactionsAsync();
            var filtered = all
                .Where(t => t.Date >= DateTime.Today)
                .OrderByDescending(t => t.Date)
                .ToList();
            FilteredTransactions = new ObservableCollection<Transaction>(filtered);
        }       
    }

}
