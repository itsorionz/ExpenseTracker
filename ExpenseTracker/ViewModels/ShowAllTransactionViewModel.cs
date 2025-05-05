using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
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
        }

        [RelayCommand]
        public async Task DownloadPdf()
        {
            await Filter();

            var pdfBytes = await _pdfService.CreatePdfAsync(FilteredTransactions.ToList());

            string filePath = Path.Combine(FileSystem.CacheDirectory, "TransactionReport.pdf");
            File.WriteAllBytes(filePath, pdfBytes);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Transaction Report",
                File = new ShareFile(filePath)
            });
        }

       
    }

}
