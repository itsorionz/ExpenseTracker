using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Services;
namespace ExpenseTracker.ViewModels;

public partial class SyncViewModel : ObservableObject
{
    private readonly SyncService _syncService;

    public SyncViewModel(SyncService syncService)
	{
        _syncService = syncService;
    }

    //[RelayCommand]
    //public async Task StartSyncAsync()
    //{
    //   await _syncService.SyncAsync();
    //}

    [RelayCommand]
    public void StartSync()
    {
        var toast = Toast.Make("Sync Service started", ToastDuration.Short, 14);
        toast.Show();
        _syncService.Sync();
    }
}