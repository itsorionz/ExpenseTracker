using ExpenseTracker.ViewModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Views 
{ 
    public partial class MonthlyCategoryWiseReportsPage : ContentPage
    {
        private readonly MonthlyCategoryWiseReportViewModel _vm;

        public MonthlyCategoryWiseReportsPage(MonthlyCategoryWiseReportViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MonthlyCategoryWiseReportViewModel vm)
            {
               await vm.LoadMonthlyCategoryPieChartAsync(vm.SelectedMonth);
            }
        }
    }
}