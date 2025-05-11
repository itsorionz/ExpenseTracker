using CommunityToolkit.Maui.Alerts;

namespace ExpenseTracker.Services
{
    public class SyncService
    {
        private readonly System.Timers.Timer _syncTimer;
        private readonly FirebaseService _firebaseService;
        private readonly DatabaseService _databaseService;
        private bool _isSyncing = false;

        public SyncService(FirebaseService firebaseService, DatabaseService databaseService)
        {
            _firebaseService = firebaseService;
            _databaseService = databaseService;

            _syncTimer = new System.Timers.Timer
            {
                Interval = 5 * 60 * 1000,
                AutoReset = true,
                Enabled = true
            };
            //_syncTimer.Elapsed += async (s, e) => await TrySyncAsync();
            _syncTimer.Elapsed += (s, e) =>  TrySync();

            Connectivity.ConnectivityChanged += async (s, e) =>
            {
                if (e.NetworkAccess == NetworkAccess.Internet)
                {
                    //await TrySyncAsync();
                    TrySync();
                }
            };
        }

        private async Task TrySyncAsync()
        {
            if (_isSyncing || Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;

            _isSyncing = true;
            try
            {
                await SyncAsync();
            }
            finally
            {
                _isSyncing = false;
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

        //without Async
        private void TrySync()
        {
            if (_isSyncing || Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;

            _isSyncing = true;
            try
            {
                Sync();
            }
            finally
            {
                _isSyncing = false;
            }
        }

        public void Sync()
        {
            TransactionSQLiteToFirebase();
            TransactionFirebaseToSQLite();
            CategorySQLiteToFirebase();
            CategoryFirebaseToSQLite();
        }

        public void TransactionSQLiteToFirebase()
        {
            var localUnsynced = _databaseService.GetUnsyncedTransactionsAsync().GetAwaiter().GetResult();
            foreach (var transaction in localUnsynced)
            {
                var success = _firebaseService.UploadTransaction(transaction);
                if (success)
                {
                    transaction.IsSynced = true;
                    _firebaseService.MarkTransactionAsSynced(transaction.Id);
                    _databaseService.UpdateTransactionAsync(transaction).GetAwaiter().GetResult();
                }
            }
        }
        public void TransactionFirebaseToSQLite()
        {
            var firebaseUnsynced = _firebaseService.GetUnsyncedTransactions();
            foreach (var transaction in firebaseUnsynced)
            {
                transaction.IsSynced = true;
                _databaseService.SaveTransactionAsync(transaction).GetAwaiter().GetResult();
                _firebaseService.MarkTransactionAsSynced(transaction.Id);
            }
        }
        public void CategorySQLiteToFirebase()
        {
            var localUnsynced = _databaseService.GetUnsyncedCategoryAsync().GetAwaiter().GetResult();
            foreach (var category in localUnsynced)
            {
                var success = _firebaseService.UploadCategory(category);
                if (success)
                {
                    category.IsSynced = true;
                    _firebaseService.MarkCategoryAsSynced(category.Id);
                    _databaseService.UpdateCategoryAsync(category).GetAwaiter().GetResult();
                }
            }
        }
        public void CategoryFirebaseToSQLite()
        {
            var firebaseUnsynced = _firebaseService.GetUnsyncedCategories();
            foreach (var category in firebaseUnsynced)
            {
                category.IsSynced = true;
                _databaseService.SaveCategoryAsync(category).GetAwaiter().GetResult();
                _firebaseService.MarkCategoryAsSynced(category.Id);
            }
        }

    }

}
