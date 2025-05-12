using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class AddFarmerPage : Page
    {
        public event EventHandler<User> FarmerAdded;
        private readonly AppDbContext _dbContext;

        public AddFarmerPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                // Check if email already exists
                if (_dbContext.Users.Any(u => u.Email == txtEmail.Text))
                {
                    MessageBox.Show("A user with this email already exists.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create new farmer
                var newFarmer = new User
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Role = "Farmer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Farmer@12"),
                    DateRegistered = DateTime.UtcNow
                };

                _dbContext.Users.Add(newFarmer);
                _dbContext.SaveChanges();

                FarmerAdded?.Invoke(this, newFarmer);
                MessageBox.Show("Farmer added successfully!", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate back after successful save
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Database error: {ex.InnerException?.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving farmer: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error",
                             MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                             MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}