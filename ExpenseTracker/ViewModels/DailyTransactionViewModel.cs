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
        [ObservableProperty]
        private DateTime selectedDate = DateTime.Today;
        [ObservableProperty]
        private decimal totalBalance;
        

        public DailyTransactionViewModel(DatabaseService db)
        {
            _db = db;
            LoadTransactionsCommand.Execute(selectedDate);
        }

        [RelayCommand]
        public async Task LoadTransactions(DateTime date)
        {
            var all = await _db.GetTransactionsAsync();
            var filtered = all
                .Where(t => t.Date == date)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.CreatedDate)
                .ToList();
            
            FilteredTransactions = new ObservableCollection<Transaction>(filtered);
            TotalBalance = filtered.Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            LoadTransactionsCommand.Execute(value);
        }

        [RelayCommand]
        public async Task Delete(Transaction transaction)
        {
            if (transaction == null) return;
            transaction.IsDeleted = true;
            transaction.DeletedBy = "User";
            transaction.DeletedDate = DateTime.Now;
            transaction.IsSynced = false;
            await _db.UpdateTransactionAsync(transaction);
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
