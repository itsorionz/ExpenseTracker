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
                await Toast.Make("Data Sync completed", ToastDuration.Short, 14).Show();
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ResetLocalLibrary()
    {
        try
        {
            IsBusy = true;
            await Task.Run(() => _syncNowService.ResetLocalLibrary());
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make("Reset Local Library Successfully", ToastDuration.Short, 14).Show();
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ResetLocalTrasaction()
    {
        try
        {
            IsBusy = true;
            await Task.Run(() => _syncNowService.ResetLocalTransaction());
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make("Reset Local Transaction Successfully", ToastDuration.Short, 14).Show();
            });
        }
        finally 
        {
            IsBusy = false;
        }        
    }

    [RelayCommand]
    public async Task ResetFirebaseLibrary()
    {
        try
        {
            IsBusy = true;
            await Task.Run(() => _syncNowService.ResetFirebaseLibrary());
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make("Reset Cloud Library Successfully", ToastDuration.Short, 14).Show();
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ResetFirebaseTransaction()
    {
        try
        {
            IsBusy = true;
            await Task.Run(() => _syncNowService.ResetFirebaseTransaction());
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Toast.Make("Reset Cloud Transaction Successfully", ToastDuration.Short, 14).Show();
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

   
}