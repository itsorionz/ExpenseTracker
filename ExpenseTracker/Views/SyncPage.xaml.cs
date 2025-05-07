using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{
    public partial class SyncPage : ContentPage
    {
        private readonly SyncViewModel _vm;

        public SyncPage(SyncViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }
    }
}