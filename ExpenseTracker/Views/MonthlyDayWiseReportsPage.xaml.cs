using ExpenseTracker.ViewModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Views 
{ 
    public partial class MonthlyDayWiseReportPage : ContentPage
    {
        private readonly MonthlyDayWiseReportViewModel _vm;

        public MonthlyDayWiseReportPage(MonthlyDayWiseReportViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MonthlyDayWiseReportViewModel vm)
            {
               await vm.LoadDailyChartAsync();
            }
        }
    }
}