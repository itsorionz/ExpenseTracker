using CommunityToolkit.Maui;
using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;
using ExpenseTracker.Views;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMicrocharts()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>(s => new DatabaseService());
            builder.Services.AddSingleton<PdfService>();
            builder.Services.AddSingleton<FirebaseService>();
            builder.Services.AddSingleton<SyncService>();
            builder.Services.AddSingleton<SyncNowService>();


            builder.Services.AddTransient<SyncPage>();
            builder.Services.AddTransient<SyncViewModel>();
            builder.Services.AddTransient<SyncNowPage>();
            builder.Services.AddTransient<SyncNowViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<AddTransactionPage>();
            builder.Services.AddTransient<AddTransactionViewModel>();
            builder.Services.AddTransient<UpdateTransactionViewModel>();
            builder.Services.AddTransient<UpdateTransactionViewModel>();
            builder.Services.AddTransient<DailyTransactionPage>();
            builder.Services.AddTransient<DailyTransactionViewModel>();
            builder.Services.AddTransient<ShowAllTransactionPage>();
            builder.Services.AddTransient<ShowAllTransactionViewModel>();
            builder.Services.AddTransient<MonthlyDayWiseReportPage>();
            builder.Services.AddTransient<MonthlyDayWiseReportViewModel>();
            builder.Services.AddTransient<MonthlyCategoryWiseReportsPage>();
            builder.Services.AddTransient<MonthlyCategoryWiseReportViewModel>();
            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
