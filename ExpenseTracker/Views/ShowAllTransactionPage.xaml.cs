using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views 
{ 
    public partial class ShowAllTransactionPage : ContentPage
    {
        public ShowAllTransactionPage(ShowAllTransactionViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ShowAllTransactionViewModel vm)
            {
                vm.FilterCommand.Execute(null);
            }
        }
    }
}