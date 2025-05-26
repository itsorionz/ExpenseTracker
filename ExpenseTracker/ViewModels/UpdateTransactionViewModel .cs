using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using System.Collections.ObjectModel;

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

        [ObservableProperty]
        public ObservableCollection<Category> categories = new();

        [ObservableProperty]
        private string selectedType;

        [ObservableProperty]
        private Category selectedCategory;

        [ObservableProperty]
        private decimal amount;

        [ObservableProperty]
        private DateTime date;

        [ObservableProperty]
        private string notes;

        private Transaction _transaction;

        public Transaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                if (_transaction != null)
                {
                    LoadTransaction(_transaction);
                }
            }
        }

        partial void OnSelectedTypeChanged(string value)
        {
            LoadCategoriesByType();
        }

        [RelayCommand]
        public async Task LoadCategoriesByType()
        {
            var allCategories = await _db.GetCategoryAsync();
            var filtered = allCategories
                .Where(c => c.Type == SelectedType)
                .OrderBy(c => c.Id)
                .ToList();

            Categories.Clear();
            foreach (var cat in filtered)
                Categories.Add(cat);
            SelectedCategory = Categories.FirstOrDefault();
        }

        public async void LoadTransaction(Transaction transaction)
        {
            SelectedType = transaction.Type;
            await LoadCategoriesByType();
            SelectedCategory = Categories.FirstOrDefault(c => c.CategoryName == transaction.Category);
            Amount = transaction.Amount;
            Date = transaction.Date;
            Notes = transaction.Notes;
        }

        [RelayCommand]
        public async Task Save()
        {
            if (string.IsNullOrWhiteSpace(SelectedType) || Amount <= 0 || SelectedCategory == null)
                return;

            _transaction.Type = SelectedType;
            _transaction.Category = SelectedCategory.CategoryName;
            _transaction.Amount = Amount;
            _transaction.Date = Date;
            _transaction.Notes = Notes;
            _transaction.IsSynced = false;
            _transaction.UpdatedBy = "User";
            _transaction.UpdatedDate = DateTime.Now;

            await _db.UpdateTransactionAsync(_transaction);
            await Shell.Current.GoToAsync("..");
        }
    }
}
