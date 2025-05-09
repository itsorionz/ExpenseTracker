using ExpenseTracker.Models;
using Newtonsoft.Json;
using System.Text;

namespace ExpenseTracker.Services
{
    public class FirebaseService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl = "https://expensetracker-7cbc5-default-rtdb.firebaseio.com/";

        public async Task<bool> UploadTransactionAsync(Transaction transaction)
        {
            if (transaction == null || transaction.Id == 0)
                return false;

            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}transactions/{transaction.Id}.json", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Transaction>> GetUnsyncedTransactionsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}transactions.json?orderBy=\"IsSynced\"&equalTo=false");
            if (!response.IsSuccessStatusCode) return new List<Transaction>();

            var json = await response.Content.ReadAsStringAsync();
            var dict = JsonConvert.DeserializeObject<Dictionary<string, Transaction>>(json);

            return dict?.Values.Select(x => new Transaction
            {
                Id = x.Id,
                Amount = x.Amount,
                Category = x.Category,
                Date = x.Date,
                Notes = x.Notes,
                Type = x.Type,
                IsSynced = true
            }).ToList() ?? new List<Transaction>();
        }

        public async Task<bool> MarkAsSyncedAsync(int id)
        {
            var patch = JsonConvert.SerializeObject(new { IsSynced = true });
            var content = new StringContent(patch, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{_baseUrl}transactions/{id}.json", content);
            return response.IsSuccessStatusCode;
        }
    }


}
