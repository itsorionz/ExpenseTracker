using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _vm;

        public HomePage(HomeViewModel vm, DatabaseService db)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadTransactions();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddTransactionPage));
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(UpdateTransactionPage));
        }
    }
}
