using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
    public async Task StartSync()
    {
        try
        {
            IsBusy = true;
            await Task.Run(() => _syncNowService.Sync());
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make("Sync completed", ToastDuration.Short, 14).Show();
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ResetLocalDatabase()
    {
        try
        {
            IsBusy = true;
            await Task.Run(() => _syncNowService.ResetSQLite());
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make("Reset Local Successfully", ToastDuration.Short, 14).Show();
            });
        }
        finally 
        {
            IsBusy = false;
        }        
    }

    [RelayCommand]
    public async Task ResetCloudDatabase()
    {
        try
        {
            IsBusy = true;
            await Task.Run(() => _syncNowService.ResetFirebase());
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make("Reset Firebase Successfully", ToastDuration.Short, 14).Show();
            });
        }
        finally
        {
            IsBusy = false;
        }
    }
}