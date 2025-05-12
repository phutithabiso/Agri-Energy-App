using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class ProductSearchPage : Page
    {
        private readonly User _currentUser;
        private readonly AppDbContext _dbContext;
        private List<Product> _allProducts;

        public ProductSearchPage(User currentUser, AppDbContext dbContext)
        {
            InitializeComponent();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            Loaded += ProductSearchPage_Loaded;
        }

        private void ProductSearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Load all products for the current farmer
                _allProducts = _dbContext.Products
                    .Where(p => p.FarmerId == _currentUser.Id)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = txtSearchQuery.Text.ToLower();

                // Skip if placeholder text is still there
                if (query == "enter product name...") return;

                // Filter products
                var filtered = _allProducts
                    .Where(p => p.Name.ToLower().Contains(query) ||
                               p.Category.ToLower().Contains(query) ||
                               p.Description?.ToLower().Contains(query) == true)
                    .ToList();

                lstSearchResults.ItemsSource = filtered;

                if (!filtered.Any())
                {
                    MessageBox.Show("No products found matching your search.",
                                  "No Results",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching products: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void TxtSearchQuery_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchQuery.Text == "Enter product name...")
            {
                txtSearchQuery.Text = "";
                txtSearchQuery.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TxtSearchQuery_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchQuery.Text))
            {
                txtSearchQuery.Text = "Enter product name...";
                txtSearchQuery.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}