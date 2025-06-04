using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{
    public partial class TotalBalancePage : ContentPage
    {
        private readonly TotalBalanceModel _vm;

        public TotalBalancePage(TotalBalanceModel vm, DatabaseService db)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is TotalBalanceModel vm)
            {
                vm.LoadTransactions();
            }
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
