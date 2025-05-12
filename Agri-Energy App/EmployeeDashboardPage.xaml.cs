using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class EmployeeDashboardPage : Page
    {
        private readonly User _currentUser;
        private readonly AppDbContext _dbContext;

        public EmployeeDashboardPage(User currentUser, AppDbContext dbContext)
        {
            InitializeComponent();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Loaded += EmployeeDashboardPage_Loaded;
        }

        private void EmployeeDashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDashboardData();
                txtWelcome.Text = $"Welcome back, {_currentUser.FirstName}! Here's your dashboard overview.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        public void RefreshData()
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                // Get farmers count
                var farmersCount = _dbContext.Users
                    .AsNoTracking()
                    .Where(u => u.Role == "Farmer")
                    .Count();
                txtFarmersCount.Text = farmersCount.ToString();

                // Get products count
                var productsCount = _dbContext.Products
                    .AsNoTracking()
                    .Count();
                txtProductsCount.Text = productsCount.ToString();

                // Get recent farmers (last 5 registered)
                var recentFarmers = _dbContext.Users
                    .AsNoTracking()
                    .Where(u => u.Role == "Farmer")
                    .OrderByDescending(u => u.DateRegistered)
                    .Take(5)
                    .ToList();
                lstRecentFarmers.ItemsSource = recentFarmers;

                // Get recent products (last 5 added)
                var recentProducts = _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Farmer)
                    .OrderByDescending(p => p.ProductionDate)
                    .Take(5)
                    .ToList();
                lstRecentProducts.ItemsSource = recentProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void ViewFarmers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Window.GetWindow(this) is EmployeeDashboardWindow parentWindow)
                {
                    //parentWindow.BtnViewFarmers_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to farmers: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void ViewProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Window.GetWindow(this) is EmployeeDashboardWindow parentWindow)
                {
                    //parentWindow.BtnViewProducts_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to products: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
    }
}