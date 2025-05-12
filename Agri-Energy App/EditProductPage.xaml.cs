using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Agri_Energy_App
{
    public partial class EditProductPage : Page, INotifyPropertyChanged
    {
        private Product _currentProduct;

        public Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                OnPropertyChanged(nameof(CurrentProduct));
            }
        }

        public List<string> Categories { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<Product> ProductUpdated;
        public event EventHandler EditCancelled;

        public EditProductPage(Product productToEdit)
        {
            InitializeComponent();

            // Initialize categories
            Categories = new List<string>
            {
                "Vegetables",
                "Poultry",
                "Fruits",
                "Dairy"
            };

            // Clone the product to avoid editing the original reference directly
            CurrentProduct = new Product
            {
                Id = productToEdit.Id,
                Name = productToEdit.Name,
                Category = productToEdit.Category,
                ProductionDate = productToEdit.ProductionDate,
                Description = productToEdit.Description,
                FarmerId = productToEdit.FarmerId,
                Farmer = productToEdit.Farmer
            };

            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(CurrentProduct.Name))
            {
                MessageBox.Show("Product name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentProduct.Category))
            {
                MessageBox.Show("Category is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentProduct.ProductionDate == default)
            {
                MessageBox.Show("Production date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProductUpdated?.Invoke(this, CurrentProduct);
            NavigationService?.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            EditCancelled?.Invoke(this, EventArgs.Empty);
            NavigationService?.GoBack();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: can be removed if not needed
        }
    }
}
