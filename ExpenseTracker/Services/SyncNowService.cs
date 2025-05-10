using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ExpenseTracker.Services;

namespace ExpenseTracker.Services
{
    public class SyncNowService
    {
        private readonly FirebaseService _firebaseService;
        private readonly DatabaseService _databaseService;

        public SyncNowService(FirebaseService firebaseService, DatabaseService databaseService)
        {
            _firebaseService = firebaseService;
            _databaseService = databaseService;
        }

        public void Sync()
        {
            SQLiteToFirebase();
            FirebaseToSQLite();
            var toast = Toast.Make("Sync complete", ToastDuration.Short, 14);
            toast.Show();
        }

        public void SQLiteToFirebase()
        {
            var localUnsynced = _databaseService.GetUnsyncedTransactionsAsync().GetAwaiter().GetResult();
            foreach (var transaction in localUnsynced)
            {
                var success = _firebaseService.UploadTransaction(transaction);
                if (success)
                {
                    transaction.IsSynced = true;
                    _firebaseService.MarkAsSynced(transaction.Id);
                    _databaseService.UpdateTransactionAsync(transaction).GetAwaiter().GetResult();
                }
            }
        }

        public void FirebaseToSQLite()
        {
            var firebaseUnsynced = _firebaseService.GetUnsyncedTransactions();
            foreach (var transaction in firebaseUnsynced)
            {
                transaction.IsSynced = true;
                _databaseService.SaveTransactionAsync(transaction).GetAwaiter().GetResult();
                _firebaseService.MarkAsSynced(transaction.Id);
            }
        }

        public void ResetFirebase()
        {
            _firebaseService.ResetFirebase();
            var toast = Toast.Make("Reset Successfully", ToastDuration.Short, 14);
            toast.Show();
        }

        public void ResetSQLite()
        {
            _databaseService.ResetDatabase();
            var toast = Toast.Make("Reset Successfully", ToastDuration.Short, 14);
            toast.Show();
        }

    }
}
