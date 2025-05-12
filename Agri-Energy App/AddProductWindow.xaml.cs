using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class AddProductWindow : Window
    {
        private readonly User _farmer;
        public event EventHandler ProductAdded;

        public AddProductWindow(User farmer)
        {
            InitializeComponent();
            _farmer = farmer;
            dpProductionDate.SelectedDate = DateTime.Today;
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
                using var dbContext = new AppDbContext();

#pragma warning disable CS8601 // Possible null reference assignment.
                var product = new Product
                {
                    Name = txtProductName.Text,
                    Category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    ProductionDate = dpProductionDate.SelectedDate ?? DateTime.Today,
                    Description = txtDescription.Text,
                    FarmerId = _farmer.Id
                };
#pragma warning restore CS8601 // Possible null reference assignment.

                dbContext.Products.Add(product);
                dbContext.SaveChanges();

                ProductAdded?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}