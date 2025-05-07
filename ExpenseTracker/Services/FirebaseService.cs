using ExpenseTracker.Models;
using Newtonsoft.Json;
using System.Text;

namespace ExpenseTracker.Services
{
    public class FirebaseService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl = "https://expensetracker-7cbc5-default-rtdb.firebaseio.com";

        public async Task<bool> UploadTransactionAsync(Transaction transaction)
        {
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{_baseUrl}/transactions.json";
            var response = await _httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Transaction>> DownloadTransactionsAsync()
        {
            var url = $"{_baseUrl}transactions.json";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<Transaction>();
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json) || json == "null")
                return new List<Transaction>();
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, Transaction>>(json);
            return dictionary?.Values.ToList() ?? new List<Transaction>();
        }

    }

}
