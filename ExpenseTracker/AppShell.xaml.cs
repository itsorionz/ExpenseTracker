using ExpenseTracker.Views;

namespace ExpenseTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(AddTransactionPage), typeof(AddTransactionPage));
            Routing.RegisterRoute(nameof(DailyTransactionPage), typeof(DailyTransactionPage));
            Routing.RegisterRoute(nameof(ShowAllTransactionPage), typeof(ShowAllTransactionPage));
            Routing.RegisterRoute(nameof(UpdateTransactionPage), typeof(UpdateTransactionPage));
            Routing.RegisterRoute(nameof(MonthlyReportPage), typeof(MonthlyReportPage));
        }
    }
}
