using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class FarmerProfilePage : Page
    {
        private User _currentUser;  // Removed readonly
        private readonly AppDbContext _dbContext;

        public FarmerProfilePage(User user, AppDbContext dbContext)
        {
            InitializeComponent();
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            Loaded += FarmerProfilePage_Loaded;
        }

        private async void FarmerProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentUser?.Id == null)
                {
                    MessageBox.Show("User ID is missing", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var updatedUser = await _dbContext.Users.FindAsync(_currentUser.Id);

                if (updatedUser == null)
                {
                    MessageBox.Show("User not found in database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Update properties instead of reassigning
                _currentUser.FirstName = updatedUser.FirstName;
                _currentUser.LastName = updatedUser.LastName;
                _currentUser.Email = updatedUser.Email;
                _currentUser.DateRegistered = updatedUser.DateRegistered;

                // Populate UI
                txtFirstName.Text = _currentUser.FirstName;
                txtLastName.Text = _currentUser.LastName;
                txtEmail.Text = _currentUser.Email;
                txtMemberSince.Text = _currentUser.DateRegistered.ToString("dd MMMM yyyy");
                txtStatus.Text = $"Welcome, {_currentUser.FirstName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Rest of your methods remain the same...
        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                _currentUser.FirstName = txtFirstName.Text.Trim();
                _currentUser.LastName = txtLastName.Text.Trim();

                _dbContext.Entry(_currentUser).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                txtStatus.Text = "Changes saved successfully!";
                MessageBox.Show("Your profile has been updated.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Error saving changes";
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                errors.Add("First name is required");
            else if (txtFirstName.Text.Trim().Length > 50)
                errors.Add("First name cannot exceed 50 characters");

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
                errors.Add("Last name is required");
            else if (txtLastName.Text.Trim().Length > 50)
                errors.Add("Last name cannot exceed 50 characters");

            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var changePasswordWindow = new ChangePasswordWindow(_currentUser);
            changePasswordWindow.Owner = Window.GetWindow(this);
            changePasswordWindow.ShowDialog();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                var dashboardWindow = new FarmerDashboardWindow(_currentUser);
                dashboardWindow.Show();
                Window.GetWindow(this)?.Close();
            }
        }
    }
}