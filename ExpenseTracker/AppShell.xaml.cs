using ExpenseTracker.Views;

namespace ExpenseTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SyncPage), typeof(SyncPage));
            Routing.RegisterRoute(nameof(SyncNowPage), typeof(SyncNowPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(AddTransactionPage), typeof(AddTransactionPage));
            Routing.RegisterRoute(nameof(DailyTransactionPage), typeof(DailyTransactionPage));
            Routing.RegisterRoute(nameof(ShowAllTransactionPage), typeof(ShowAllTransactionPage));
            Routing.RegisterRoute(nameof(UpdateTransactionPage), typeof(UpdateTransactionPage));
            Routing.RegisterRoute(nameof(MonthlyDayWiseReportPage), typeof(MonthlyDayWiseReportPage));
            Routing.RegisterRoute(nameof(MonthlyCategoryWiseReportsPage), typeof(MonthlyCategoryWiseReportsPage));

        }
    }
}
