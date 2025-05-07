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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "expenses.db");
            builder.Services.AddSingleton<DatabaseService>(s => new DatabaseService(dbPath));
            builder.Services.AddSingleton<PdfService>();
            builder.Services.AddSingleton<FirebaseService>();
            builder.Services.AddSingleton<SyncService>();
            

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
            builder.Services.AddTransient<MonthlyReportPage>();
            builder.Services.AddTransient<MonthlyReportViewModel>();
            builder.Services.AddTransient<SyncPage>();
            builder.Services.AddTransient<SyncViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
