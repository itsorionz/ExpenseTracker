using SQLite;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "expenses.db");

        public DatabaseService()
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Transaction>().Wait();
            _database.CreateTableAsync<Category>().Wait();
        }

        public async Task<bool> IsDatabaseEmptyAsync()
        {
            var count = await _database.Table<Transaction>().CountAsync();
            return count == 0;
        }

        public Task<List<Transaction>> GetUnsyncedTransactionsAsync()
        {
            return _database.Table<Transaction>().Where(t => !t.IsSynced).ToListAsync();
        }

        public Task<List<Transaction>> GetTransactionsAsync()
        {
            return _database.Table<Transaction>().Where(t => !t.IsDeleted).OrderByDescending(t => t.Date ).ToListAsync();
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
        
        public Task<List<Category>> GetUnsyncedCategoryAsync()
        {
            return _database.Table<Category>().Where(t => !t.IsSynced).ToListAsync();
        }
        public Task<List<Category>> GetCategoryAsync()
        {
            return _database.Table<Category>().Where(t => !t.IsDeleted).ToListAsync();
        }

        public Task<int> SaveCategoryAsync(Category category)
        {
            return _database.InsertAsync(category);
        }

        public Task<int> UpdateCategoryAsync(Category category)
        {
            return _database.UpdateAsync(category);
        }

        public Task<int> DeleteCategoryAsync(Category category)
        {
            return _database.DeleteAsync(category);
        }
        public async Task ResetDatabaseAsync()
        {
            await _database.DropTableAsync<Transaction>();
            await _database.CreateTableAsync<Transaction>();
            await _database.DropTableAsync<Category>();
            await _database.CreateTableAsync<Category>();
        }
        public void ResetTransaction()
        {
            _database.DropTableAsync<Transaction>().Wait();
            _database.CreateTableAsync<Transaction>().Wait();
        }
        public void ResetLibrary()
        {
            _database.DropTableAsync<Category>().Wait();
            _database.CreateTableAsync<Category>().Wait();
        }
    }
}
