using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ExpenseTracker.Services;
using ExpenseTracker.Models;
using ExpenseTracker.Views;

public partial class AddTransactionViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public ObservableCollection<Category> Categories { get; set; } = new();
    [ObservableProperty] 
    private string selectedType;
    [ObservableProperty] 
    private Category selectedCategory;
    [ObservableProperty] 
    private decimal amount;
    [ObservableProperty] 
    private DateTime date = DateTime.Today;
    [ObservableProperty] 
    private string notes;

    public AddTransactionViewModel(DatabaseService db)
    {
        _db = db;
        SelectedType = "Expense";
        LoadCategoriesByType();
    }

    [RelayCommand]
    public async void LoadCategoriesByType()
    {
        var allCategories = await _db.GetCategoryAsync();
        var filtered = allCategories
                        .Where(c => c.Type == SelectedType)
                        .OrderBy(a => a.Id)
                        .ToList();
        Categories.Clear();
        foreach (var cat in filtered)
            Categories.Add(cat);
        SelectedCategory = Categories.FirstOrDefault();
    }

    partial void OnSelectedTypeChanged(string value)
    {
        LoadCategoriesByType();
    }

    [RelayCommand]
    public async Task Save()
    {
        if (string.IsNullOrWhiteSpace(SelectedType) || Categories.Count <= 0 || Amount <= 0)
            return;
        var transaction = new Transaction
        {
            Type = SelectedType,
            Category = SelectedCategory.CategoryName,
            Amount = Amount,
            Date = Date,
            Notes = Notes,
            CreatedBy = "User",
            CreatedDate = DateTime.Now,
            IsSynced = false,
        };
        if (transaction.Id > 0)
            await _db.UpdateTransactionAsync(transaction);
        else
            await _db.SaveTransactionAsync(transaction);

        await Shell.Current.GoToAsync(nameof(HomePage));
    }
}
