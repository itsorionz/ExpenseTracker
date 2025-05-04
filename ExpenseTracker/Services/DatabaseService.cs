﻿using SQLite;
using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Transaction>().Wait();
            //_database.DropTableAsync<Transaction>();
        }

        //public Task<int> DeleteAllTransactionsAsync()
        //{
        //    return _database.DeleteAllAsync<Transaction>();
        //}

        //public async Task ResetTransactionTableAsync()
        //{
        //    await _database.DropTableAsync<Transaction>();
        //    await _database.CreateTableAsync<Transaction>();
        //}
        public Task<List<Transaction>> GetTransactionsAsync()
        {
            return _database.Table<Transaction>().OrderByDescending(t => t.Date).ToListAsync();
        }

        public Task<int> SaveTransactionAsync(Transaction transaction)
        {
            if (transaction.Id != 0)
            {
                return _database.UpdateAsync(transaction);
            }
            else
            {
                return _database.InsertAsync(transaction);
            }
        }
        public Task<int> UpdateTransactionAsync(Transaction transaction)
        {
            return _database.UpdateAsync(transaction);
        }
        public Task<int> DeleteTransactionAsync(Transaction transaction)
        {
           return _database.DeleteAsync(transaction);
        }
    }
}
