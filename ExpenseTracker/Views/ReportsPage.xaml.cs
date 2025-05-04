using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views 
{ 
    public partial class ReportPage : ContentPage
    {
        public ReportPage(ReportViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}