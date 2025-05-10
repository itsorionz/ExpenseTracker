using ExpenseTracker.Services;

namespace ExpenseTracker
{
    public partial class App : Application
    {
        //private readonly SyncService _syncService;

        public App(SyncService syncService)
        {
            InitializeComponent();
            //_syncService = syncService;        
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}