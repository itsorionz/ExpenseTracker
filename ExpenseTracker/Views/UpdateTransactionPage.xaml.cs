using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

[QueryProperty(nameof(Transaction), "Transaction")]
public partial class UpdateTransactionPage : ContentPage, IQueryAttributable
{
    private readonly UpdateTransactionViewModel _vm;

    public UpdateTransactionPage(UpdateTransactionViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Transaction", out var value) && value is Transaction transaction)
        {
            _vm.LoadTransaction(transaction);
        }
    }
}
