using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class AddProductPage : Page
    {
        private readonly User _farmer;
        private readonly AppDbContext _dbContext;
        public event EventHandler ProductAdded;

        public AddProductPage(User farmer, AppDbContext dbContext)
        {
            InitializeComponent();
            _farmer = farmer ?? throw new ArgumentNullException(nameof(farmer));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            dpProductionDate.SelectedDate = DateTime.Today;
            cmbCategory.SelectedIndex = 0;
        }

        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Please enter a product name", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var product = new Product
                {
                    Name = txtProductName.Text,
                    Category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                    ProductionDate = dpProductionDate.SelectedDate ?? DateTime.Today,
                    Description = txtDescription.Text,
                    FarmerId = _farmer.Id
                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                ProductAdded?.Invoke(this, EventArgs.Empty);

                // Navigate back after successful save
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}