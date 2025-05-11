using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels
{
    [QueryProperty(nameof(Category), "Category")]
    public partial class UpdateCategoryViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        public UpdateCategoryViewModel(DatabaseService db)
        {
            _db = db;
        }

        private Category category;

        public Category Category
        {
            get => category;
            set
            {
                category = value;
                if (category != null)
                {
                    SelectedType = category.Type;
                    CategoryName = category.CategoryName;
                }
            }
        }

        [ObservableProperty] 
        private string selectedType;
        [ObservableProperty] 
        private string categoryName;


        private Category _originalCategory;

        public void LoadCategory(Category category)
        {
            _originalCategory = category;
            SelectedType = category.Type;
            CategoryName = category.CategoryName;
        }

        [RelayCommand]
        public async Task Save()
        {
            if (string.IsNullOrWhiteSpace(SelectedType))
                return;
            Category.Type = SelectedType;
            Category.CategoryName = CategoryName;
            await _db.UpdateCategoryAsync(Category);
            await Shell.Current.GoToAsync("..");
        }
    }
}
