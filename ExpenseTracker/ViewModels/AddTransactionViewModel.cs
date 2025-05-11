using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ExpenseTracker.Services;
using ExpenseTracker.Models;

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
    private async void LoadCategoriesByType()
    {
        var allCategories = await _db.GetCategoryAsync();
        var filtered = allCategories
                        .Where(c => c.Type == SelectedType)
                        .ToList();
        Categories.Clear();
        foreach (var cat in filtered)
            Categories.Add(cat);

        SelectedCategory = Categories.FirstOrDefault();
    }

    partial void OnSelectedTypeChanged(string value)
    {
        LoadCategoriesByType(); // reload when type changes
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
            Notes = Notes
        };
        if (transaction.Id > 0)
            await _db.UpdateTransactionAsync(transaction);
        else
            await _db.SaveTransactionAsync(transaction);

        await Shell.Current.GoToAsync("..");
    }
}
