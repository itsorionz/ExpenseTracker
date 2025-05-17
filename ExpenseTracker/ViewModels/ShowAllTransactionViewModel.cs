using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Views;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModels
{
    public partial class ShowAllTransactionViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        private readonly PdfService _pdfService;

        [ObservableProperty] private DateTime startDate = DateTime.Today.AddDays(-30);
        [ObservableProperty] private DateTime endDate = DateTime.Today;
        [ObservableProperty] private ObservableCollection<Transaction> filteredTransactions = new();
        [ObservableProperty]
        private decimal totalBalance;

        public ShowAllTransactionViewModel(DatabaseService db, PdfService pdfService)
        {
            _db = db;
            _pdfService = pdfService;
        }

        [RelayCommand]
        public async Task Filter()
        {
            var all = await _db.GetTransactionsAsync();
            var filtered = all
                .Where(t => t.Date >= StartDate && t.Date <= EndDate)
                .OrderByDescending(t => t.Date)
                .ToList();
            FilteredTransactions = new ObservableCollection<Transaction>(filtered);
            TotalBalance = filtered.Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);
        }

        [RelayCommand]
        public async Task DownloadPdf()
        {
            await Filter();
            string path = await _pdfService.CreatePdfAsync(FilteredTransactions.ToList());

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(path)
            });
        }

        [RelayCommand]
        public async Task Delete(Transaction transaction)
        {
            if (transaction == null) return;
            transaction.IsDeleted = true;
            transaction.DeletedBy = "User";
            transaction.DeletedDate = DateTime.Now;
            transaction.IsSynced = false;
            await _db.UpdateTransactionAsync(transaction);
            FilteredTransactions.Remove(transaction);
        }

        [RelayCommand]
        public async Task Update(Transaction transaction)
        {
            if (transaction == null) return;
            var navParam = new Dictionary<string, object> { { "Transaction", transaction } };
            await Shell.Current.GoToAsync(nameof(UpdateTransactionPage), navParam);
        }

    }

}
