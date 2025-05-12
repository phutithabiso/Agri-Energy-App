using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Agri_Energy_App
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _dbContext = new AppDbContext();

        public MainWindow()
        {
            InitializeComponent();
            _dbContext.Database.EnsureCreated();

            // Set focus to email field when window loads
            Loaded += (s, e) => txtEmail.Focus();
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                // Retrieve user from database by email
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == txtEmail.Text);

                // If user doesn't exist or password doesn't match
                if (user == null || !PasswordHasher.VerifyPassword(txtPassword.Password, user.PasswordHash))
                {
                    ShowError("Invalid email or password.");
                    return;
                }

                // Update the LastLogin field with current date and time
                user.LastLogin = DateTime.UtcNow;  
                await _dbContext.SaveChangesAsync();

                // Check if password needs rehashing (if work factor increased)
                if (PasswordHasher.NeedsRehash(user.PasswordHash))
                {
                    user.PasswordHash = PasswordHasher.HashPassword(txtPassword.Password);
                    await _dbContext.SaveChangesAsync();
                }

                // Store user session if needed
                UserSession.CurrentUser = user;

                // Redirect based on user role
                Window dashboardWindow;
                switch (user.Role?.ToLower())
                {
                    case "farmer":
                        dashboardWindow = new FarmerDashboardWindow(user);
                        break;
                    case "employee":
                        dashboardWindow = new EmployeeDashboardWindow(user);
                        break;
                    default:
                        ShowError("Your account has an invalid role configuration.");
                        return;
                }

                dashboardWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Login failed: {ex.Message}");
              
                Console.WriteLine($"Login error: {ex}");
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowError("Please enter your email address.");
                txtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                ShowError("Please enter a valid email address.");
                txtEmail.Focus();
                txtEmail.SelectAll();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                ShowError("Please enter your password.");
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var resetWindow = new PasswordResetWindow(txtEmail.Text);
            resetWindow.ShowDialog();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Login Error",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignIn_Click(sender, e);
            }
        }

        // Helper class to manage user session
        public static class UserSession
        {
            public static User CurrentUser { get; set; }

            public static void Clear()
            {
                CurrentUser = null;
            }
        }
    }
}
