using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Views;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        public ObservableCollection<Transaction> transactions;

        [ObservableProperty]
        private decimal totalBalance;
        [ObservableProperty]
        private bool isBusy;

        public HomeViewModel(DatabaseService db)
        {
            _db = db;
            transactions = new ObservableCollection<Transaction>();
        }

        [RelayCommand]
        public async Task RefreshAsync()
        {
            IsBusy = true;
            await LoadTransactions();
            IsBusy = false;
        }

        public async Task LoadTransactions()
        {
            var data = await _db.GetTransactionsAsync();
            Transactions.Clear();
            foreach (var t in data.OrderByDescending(t => t.Date).Take(10))
                Transactions.Add(t);
            TotalBalance = data.Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);
        }

        [RelayCommand]
        public async Task Delete(Transaction transaction)
        {
            if (transaction == null) return;
            await _db.DeleteTransactionAsync(transaction);
            Transactions.Remove(transaction);
            TotalBalance = Transactions.Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);
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
