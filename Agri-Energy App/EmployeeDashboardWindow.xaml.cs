using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class EmployeeDashboardWindow : Window
    {
        private readonly User _currentUser;
        private readonly AppDbContext _dbContext;

        public EmployeeDashboardWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _dbContext = new AppDbContext();
            Title = $"Agri_Energy_App - Employee Dashboard ({user.FirstName} {user.LastName})";
            txtWelcome.Text = $"Welcome, {_currentUser.FirstName} {_currentUser.LastName}! (Employee)";

            // Load default dashboard view
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            var dashboardPage = new EmployeeDashboardPage(_currentUser, _dbContext);
            mainContentFrame.Navigate(dashboardPage);
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            ShowDashboard();
        }

        private void BtnAddFarmer_Click(object sender, RoutedEventArgs e)
        {
            var addFarmerWindow = new AddFarmerPage(_dbContext);
            addFarmerWindow.FarmerAdded += (s, farmer) =>
            {
                // Refresh dashboard if needed
                if (mainContentFrame.Content is EmployeeDashboardPage dashboard)
                {
                    dashboard.RefreshData();
                }
            };
            mainContentFrame.Navigate(addFarmerWindow);
        }

        private void BtnViewProducts_Click(object sender, RoutedEventArgs e)
        {
            var productsPage = new EmployeeProductsPage(_currentUser, _dbContext);
            mainContentFrame.Navigate(productsPage);
        }

        private void BtnViewFarmers_Click(object sender, RoutedEventArgs e)
        {
            var farmersPage = new FarmersListPage(_currentUser, _dbContext);
            mainContentFrame.Navigate(farmersPage);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Update last login time
                _currentUser.LastLogin = DateTime.UtcNow;
                _dbContext.SaveChanges();

                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _dbContext.Dispose();
        }
    }
}