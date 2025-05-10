using SQLite;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Transaction>().Wait();
        }

        public async Task<bool> IsDatabaseEmptyAsync()
        {
            var count = await _database.Table<Transaction>().CountAsync();
            return count == 0;
        }

        public Task<List<Transaction>> GetTransactionsAsync()
        {
            return _database.Table<Transaction>().OrderByDescending(t => t.Date).ToListAsync();
        }

        public Task<int> SaveTransactionAsync(Transaction transaction)
        {
            return _database.InsertAsync(transaction);
        }

        public Task<int> UpdateTransactionAsync(Transaction transaction)
        {
            return _database.UpdateAsync(transaction);
        }

        public Task<int> DeleteTransactionAsync(Transaction transaction)
        {
           return _database.DeleteAsync(transaction);
        }

        public Task<List<Transaction>> GetUnsyncedTransactionsAsync()
        {
            return _database.Table<Transaction>().Where(t => !t.IsSynced).ToListAsync();
        }
        public async Task ResetDatabaseAsync()
        {
            await _database.DropTableAsync<Transaction>();
        }
        public void ResetDatabase()
        {
            _database.DropTableAsync<Transaction>();
        }
    }
}
