using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Services;
namespace ExpenseTracker.ViewModels;

public partial class SyncNowViewModel : ObservableObject
{
    private readonly SyncNowService _syncNowService;
    [ObservableProperty]
    private bool isBusy;

    public SyncNowViewModel(SyncNowService syncNowService)
	{
        _syncNowService = syncNowService;
    }

    [RelayCommand]
    public void StartSync()
    {
        IsBusy = true;
        _syncNowService.Sync();
        IsBusy = false;
    }

    [RelayCommand]
    public void ResetLocalDatabase()
    {
        IsBusy = true;
        _syncNowService.ResetSQLite();
        IsBusy = false;
    }

    [RelayCommand]
    public void ReseCloudDatabase()
    {
        IsBusy = true;
        _syncNowService.ResetFirebase();
        IsBusy = false;
    }
}