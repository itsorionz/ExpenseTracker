using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.ObjectModel;
using ExpenseTracker.Services;
using ExpenseTracker.Models;

public partial class AddTransactionViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public ObservableCollection<string> Types { get; } = new() { "Income", "Expense" };

    [ObservableProperty] private string selectedType;
    [ObservableProperty] private string category;
    [ObservableProperty] private decimal amount;
    [ObservableProperty] private DateTime date = DateTime.Today;
    [ObservableProperty] private string notes;

    public AddTransactionViewModel(DatabaseService db)
    {
        _db = db;
        SelectedType = "Expense";
    }

    [RelayCommand]
    public async Task Save()
    {
        if (string.IsNullOrWhiteSpace(SelectedType) || Amount <= 0)
            return;
        var transaction = new Transaction
        {
            Type = SelectedType,
            Category = Category,
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
