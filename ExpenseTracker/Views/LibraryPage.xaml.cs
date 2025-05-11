namespace ExpenseTracker.Views;

public partial class LibraryPage : ContentPage
{
	private readonly LibraryViewModel _vm;
	public LibraryPage(LibraryViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = vm;
	}
}