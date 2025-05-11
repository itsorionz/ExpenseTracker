using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

[QueryProperty(nameof(Category), "Category")]
public partial class UpdateCategoryPage : ContentPage, IQueryAttributable
{
    private readonly UpdateCategoryViewModel _vm;

    public UpdateCategoryPage(UpdateCategoryViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Category", out var value) && value is Category category)
        {
            _vm.LoadCategory(category);
        }
    }
}
