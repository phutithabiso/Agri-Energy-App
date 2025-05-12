using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class FarmerDashboardWindow : Window
    {
        private readonly User _currentUser;
        private readonly AppDbContext _dbContext;

        public FarmerDashboardWindow(User user)
        {
            InitializeComponent();
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _dbContext = new AppDbContext();

            // Set window title and welcome message
            Title = $"AgriConnect - Farmer Dashboard ({user.FirstName} {user.LastName})";
            txtWelcome.Text = $"Welcome, {_currentUser.FirstName} {_currentUser.LastName}!";

            // Load default view
            ShowProductsPage();
        }
        
        private void ShowProductsPage()
        {
            var productsPage = new ProductsPageView(_currentUser, _dbContext);
            mainContentFrame.Navigate(productsPage);
        }


        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            ShowProductsPage();
        }
        // Event handlers for buttons
        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductPage = new AddProductPage(_currentUser, _dbContext);
            addProductPage.ProductAdded += (s, product) =>
            {
                if (mainContentFrame.Content is ProductsPageView productsPage)
                {
                    productsPage.LoadProduct();
                }
            };
            mainContentFrame.Navigate(addProductPage);
        }
        // Event handler for the "View Products" button
        private void BtnSearchProducts_Click(object sender, RoutedEventArgs e)
        {
            var searchPage = new ProductSearchPage(_currentUser, _dbContext);
            mainContentFrame.Navigate(searchPage);
        }

        // Event handler for the "View Farmers" button
        private void btnFarmerDetails_Click(object sender, RoutedEventArgs e)
        {
            var profilePage = new FarmerProfilePage(_currentUser, _dbContext);
            mainContentFrame.Navigate(profilePage);
        }

        // Event handler for the "View Transactions" button
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?",
                                      "Confirm Logout",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Update last login time
                _currentUser.LastLogin = DateTime.UtcNow;
                _dbContext.SaveChanges();

                // Return to login window
                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
        }
        // Event handler for the "Close" button
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _dbContext?.Dispose();
        }
    }
}