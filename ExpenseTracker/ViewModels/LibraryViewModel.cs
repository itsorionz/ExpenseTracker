using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ExpenseTracker.Services;
using ExpenseTracker.Models;
using ExpenseTracker.Views;

public partial class LibraryViewModel : ObservableObject
{
    //private readonly DatabaseService _db;

    public LibraryViewModel(/*DatabaseService db*/)
    {
        //_db = db;
    }

    [RelayCommand]
    private async Task NavigateToCategoryPage()
    {
        await Shell.Current.GoToAsync(nameof(AddCategoryPage));
    }



}
