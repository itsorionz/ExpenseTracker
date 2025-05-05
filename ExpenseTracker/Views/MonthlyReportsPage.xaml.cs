using ExpenseTracker.ViewModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Views 
{ 
    public partial class MonthlyReportPage : ContentPage
    {
        private readonly MonthlyReportViewModel _vm;

        public MonthlyReportPage(MonthlyReportViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MonthlyReportViewModel vm)
            {
               await vm.LoadMonthlyReportAsync();
            }
        }
    }
}