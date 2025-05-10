using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Services;
namespace ExpenseTracker.ViewModels;

public partial class SyncNowViewModel : ObservableObject
{
    private readonly SyncNowService _syncNowService;

    public SyncNowViewModel(SyncNowService syncNowService)
	{
        _syncNowService = syncNowService;
    }

    [RelayCommand]
    public void StartSync()
    {
        _syncNowService.Sync();
    }

    [RelayCommand]
    public void ResetLocalDatabase()
    {
        _syncNowService.ResetSQLite();
    }

    [RelayCommand]
    public void ReseCloudDatabase()
    {
        _syncNowService.ResetFirebase();
    }
}