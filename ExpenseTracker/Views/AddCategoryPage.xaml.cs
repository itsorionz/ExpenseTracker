namespace ExpenseTracker.Views
{
    public partial class AddCategoryPage : ContentPage
    {
        private readonly AddCategoryViewModel _vm;
        public AddCategoryPage(AddCategoryViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AddCategoryViewModel vm)
            {
                vm.LoadCategories();
            }
        }
    }
}
