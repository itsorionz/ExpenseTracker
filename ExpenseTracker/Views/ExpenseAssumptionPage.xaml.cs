using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class ExpenseAssumptionPage : ContentPage
{
    private readonly ExpenseAssumptionViewModel _vm;

    public ExpenseAssumptionPage(ExpenseAssumptionViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ExpenseAssumptionViewModel vm)
        {
           vm.LoadExpenseAssumption();
        }
    }

}