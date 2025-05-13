using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class DailyTransactionPage : ContentPage
{
    private readonly DailyTransactionViewModel _vm;

    public DailyTransactionPage(DailyTransactionViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DailyTransactionViewModel vm)
        {
            vm.LoadTransactionsCommand.Execute(vm.SelectedDate);
        }
    }
}