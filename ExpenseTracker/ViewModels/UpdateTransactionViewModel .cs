using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels
{
    [QueryProperty(nameof(Transaction), "Transaction")]
    public partial class UpdateTransactionViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        public UpdateTransactionViewModel(DatabaseService db)
        {
            _db = db;
        }

        private Transaction transaction;

        public Transaction Transaction
        {
            get => transaction;
            set
            {
                transaction = value;
                if (transaction != null)
                {
                    SelectedType = transaction.Type;
                    Category = transaction.Category;
                    Amount = transaction.Amount;
                    Date = transaction.Date;
                    Notes = transaction.Notes;
                }
            }
        }

        [ObservableProperty] private string selectedType;
        [ObservableProperty] private string category;
        [ObservableProperty] private decimal amount;
        [ObservableProperty] private DateTime date;
        [ObservableProperty] private string notes;

        private Transaction _originalTransaction;

        public void LoadTransaction(Transaction transaction)
        {
            _originalTransaction = transaction;
            SelectedType = transaction.Type;
            Category = transaction.Category;
            Amount = transaction.Amount;
            Date = transaction.Date;
            Notes = transaction.Notes;
        }

        [RelayCommand]
        public async Task Save()
        {
            if (string.IsNullOrWhiteSpace(SelectedType) || Amount <= 0)
                return;
            Transaction.Type = SelectedType;
            Transaction.Category = Category;
            Transaction.Amount = Amount;
            Transaction.Date = Date;
            Transaction.Notes = Notes;
            Transaction.IsSynced = false;
            Transaction.UpdatedBy = "User";
            Transaction.UpdatedDate = DateTime.Now;
            await _db.UpdateTransactionAsync(Transaction);
            await Shell.Current.GoToAsync("..");
        }
    }
}
