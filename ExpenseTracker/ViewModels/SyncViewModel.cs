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

    [RelayCommand]
    public async Task StartSyncAsync()
    {
       await _syncService.SyncAsync();
    }
}