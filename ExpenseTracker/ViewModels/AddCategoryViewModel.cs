using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ExpenseTracker.Services;
using ExpenseTracker.Models;
using Microsoft.Maui.Storage;
using ExpenseTracker.Views;

public partial class AddCategoryViewModel : ObservableObject
{
    private readonly DatabaseService _db;
    public ObservableCollection<string> Types { get; } = new() { "Income", "Expense" };
    [ObservableProperty]
    public ObservableCollection<Category> categories;

    [ObservableProperty]
    private string selectedType;

    [ObservableProperty]
    private string categoryName;

    public AddCategoryViewModel(DatabaseService db)
    {
        _db = db;
        selectedType = "Expense";
        categories = new ObservableCollection<Category>();
    }

    public async Task LoadCategories()
    {
        var data = await _db.GetCategoryAsync();
        Categories.Clear();
        int sl = 1;
        foreach (var t in data)
        {
            t.Sl = sl++;
            Categories.Add(t);
        }
    }

    [RelayCommand]
    private async Task SaveCategoryAsync()
    {
        if (string.IsNullOrWhiteSpace(CategoryName) || string.IsNullOrWhiteSpace(SelectedType))
        {
            await Shell.Current.DisplayAlert("Validation", "Please enter a category name and select a type.", "OK");
            return;
        }
        var newCategory = new Category
        {
            Type = SelectedType,
            CategoryName = CategoryName.Trim(),
            CreatedBy = "User",
            CreatedDate = DateTime.Now
        };
        await _db.SaveCategoryAsync(newCategory);
        await Shell.Current.DisplayAlert("Success", "Category saved successfully.", "OK");
        CategoryName = string.Empty;
        SelectedType = SelectedType;
        await LoadCategories();
    }

    [RelayCommand]
    public async Task Delete(Category category)
    {
        if (category == null) return;
        category.IsDeleted = true;
        category.DeletedBy = "User";
        category.DeletedDate = DateTime.Now;
        await _db.UpdateCategoryAsync(category);
        Categories.Remove(category);
        await LoadCategories();
    }

    [RelayCommand]
    public async Task Update(Category category)
    {
        if (category == null) return;
        var navParam = new Dictionary<string, object> { { "Category", category } };
        await Shell.Current.GoToAsync(nameof(UpdateCategoryPage), navParam);
    }

}
