using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class ProductsPageView : Page
    {
        private readonly User _current;
        private readonly AppDbContext _Context;
        private List<Product> _product = new();

        public ProductsPageView(User currentUser, AppDbContext dbContext)
        {
            InitializeComponent();
            _current = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Loaded += ProductsPage_loaded;
        }

        private void ProductsPage_loaded(object sender, RoutedEventArgs e)
        {
            LoadProduct();
        }

        public void LoadProduct()
        {
            try
            {
                _product = _Context.Products
                    .Include(p => p.Farmer)
                    .Where(p => p.FarmerId == _current.Id)
                    .ToList();

                productsList.ItemsSource = _product;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void EditProducts_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int productId)
            {
                try
                {
                    var product = _product.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        var editPage = new EditProductPage(product);
                        editPage.ProductUpdated += (s, updatedProduct) =>
                        {
                            try
                            {
                                var o = _Context.Products.Find(updatedProduct.Id);
                                if (o != null)
                                {
                                    _Context.Entry(o).CurrentValues.SetValues(updatedProduct);
                                    _Context.SaveChanges();
                                    LoadProduct();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error saving changes: {ex.Message}",
                                              "Error",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                            }
                        };
                        NavigationService?.Navigate(editPage);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error editing product: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void DeleteProducts_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int productId)
            {
                var result = MessageBox.Show("Are you sure you want to delete this product?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var s= _Context.Products.Find(productId);
                        if (s != null)
                        {
                            _Context.Products.Remove(s);
                            _Context.SaveChanges();
                            LoadProduct();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting product: {ex.Message}",
                                     "Error",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
