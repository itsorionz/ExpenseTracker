using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Views;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModels
{
    public partial class DailyTransactionViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        [ObservableProperty] 
        private ObservableCollection<Transaction> filteredTransactions = new();

        public DailyTransactionViewModel(DatabaseService db)
        {
            _db = db;
        }

        [RelayCommand]
        public async Task LoadTransactions()
        {
            var all = await _db.GetTransactionsAsync();
            var filtered = all
                .Where(t => t.Date == DateTime.Today)
                .OrderByDescending(t => t.Date)
                .ToList();
            FilteredTransactions = new ObservableCollection<Transaction>(filtered);
        }

        [RelayCommand]
        public async Task Delete(Transaction transaction)
        {
            if (transaction == null) return;
            await _db.DeleteTransactionAsync(transaction);
            FilteredTransactions.Remove(transaction);            
        }

        [RelayCommand]
        public async Task Update(Transaction transaction)
        {
            if (transaction == null) return;
            var navParam = new Dictionary<string, object> { { "Transaction", transaction } };
            await Shell.Current.GoToAsync(nameof(UpdateTransactionPage), navParam);
        }
    }

}
