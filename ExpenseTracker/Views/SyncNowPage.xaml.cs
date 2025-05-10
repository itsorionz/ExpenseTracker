using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{
    public partial class SyncNowPage : ContentPage
    {
        private readonly SyncNowViewModel _vm;

        public SyncNowPage(SyncNowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }
    }
}