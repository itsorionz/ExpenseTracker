using System.Timers;
using ExpenseTracker.Services;
using ExpenseTracker.Models;
using Microsoft.Maui.Storage;

namespace ExpenseTracker.Services
{
    public class SyncService
    {
        private readonly System.Timers.Timer _syncTimer;
        private readonly FirebaseService _firebaseService;
        private readonly DatabaseService _databaseService;

        public SyncService(FirebaseService firebaseService, DatabaseService databaseService)
        {
            _firebaseService = firebaseService;
            _databaseService = databaseService;

            _syncTimer = new System.Timers.Timer
            {
                Interval = 1 * 60 * 1000,
                AutoReset = true,
                Enabled = true
            };
            _syncTimer.Elapsed += async (s, e) => await TrySyncAsync();

            Connectivity.ConnectivityChanged += async (s, e) =>
            {
                if (e.NetworkAccess == NetworkAccess.Internet)
                {
                    await SyncAsync();
                }
            };
        }

        private async Task TrySyncAsync()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                await SyncAsync();
            }
        }

        public async Task SyncAsync()
        {
            await SQLiteToFirebaseAsync();
            await FirebaseToSQLiteAsync();
        }

        public async Task SQLiteToFirebaseAsync()
        {
            var localUnsynced = await _databaseService.GetUnsyncedTransactionsAsync();
            foreach (var transaction in localUnsynced)
            {
                var success = await _firebaseService.UploadTransactionAsync(transaction);
                if (success)
                {
                    transaction.IsSynced = true;
                    await _firebaseService.MarkAsSyncedAsync(transaction.Id);
                    await _databaseService.UpdateTransactionAsync(transaction);
                }
            }
        }
        public async Task FirebaseToSQLiteAsync()
        {
            var firebaseUnsynced = await _firebaseService.GetUnsyncedTransactionsAsync();
            foreach (var transaction in firebaseUnsynced)
            {
                transaction.IsSynced = true;
                await _databaseService.SaveTransactionAsync(transaction);
                await _firebaseService.MarkAsSyncedAsync(transaction.Id);
            }
        }


    }

}
