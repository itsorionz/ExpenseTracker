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
            TransactionSQLiteToFirebase();
            CategorySQLiteToFirebase();
            TransactionFirebaseToSQLite();
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

        public void ResetFirebaseTransaction()
        {
            _firebaseService.ResetTransaction();
        }
        public void ResetFirebaseLibrary()
        {
            _firebaseService.ResetLibrary();
        }
        public void ResetLocalTransaction()
        {
            _databaseService.ResetTransaction();
        }
        public void ResetLocalLibrary()
        {
            _databaseService.ResetLibrary();
        }

    }
}
