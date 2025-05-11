namespace ExpenseTracker.Views
{
    public partial class AddTransactionPage : ContentPage
    {
        private readonly AddTransactionViewModel _vm;
        public AddTransactionPage(AddTransactionViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }
    }
}
