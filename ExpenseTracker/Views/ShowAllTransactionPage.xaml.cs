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
    }
}