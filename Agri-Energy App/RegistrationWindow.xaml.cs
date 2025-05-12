using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Agri_Energy_App
{
    public partial class RegistrationWindow : Window
    {
        private readonly AppDbContext _dbContext = new AppDbContext();

        public RegistrationWindow()
        {
            InitializeComponent();
            _dbContext.Database.EnsureCreated();

            // Set default role selection
            cmbRole.SelectedIndex = 0;
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                // Check if email already exists
                if (_dbContext.Users.Any(u => u.Email == txtEmail.Text))
                {
                    ShowError("This email is already registered.");
                    return;
                }

                // Get selected role
                string selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

                // Create and save new user
                var user = new User
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PasswordHash = PasswordHasher.HashPassword(txtPassword.Password),
                    Role = selectedRole
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                MessageBox.Show("Account created successfully! Please log in.", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Redirect to login window
                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (DbUpdateException dbEx)
            {
                ShowError("Database error. Please try again later.");
                Console.WriteLine($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                ShowError($"Registration failed: {ex.Message}");
                Console.WriteLine($"Error: {ex}");
            }
        }

        private bool ValidateInputs()
        {
            // Validate names
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                ShowError("Please enter your first and last name.");
                return false;
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                ShowError("Please enter a valid email address.");
                return false;
            }

            // Validate password
            if (string.IsNullOrWhiteSpace(txtPassword.Password) ||
                txtPassword.Password.Length < 8)
            {
                ShowError("Password must be at least 8 characters long.");
                return false;
            }

            // Validate password contains at least one number and one letter
            if (!Regex.IsMatch(txtPassword.Password, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
            {
                ShowError("Password must contain at least one letter and one number.");
                return false;
            }

            // Validate password match
            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                ShowError("Passwords do not match.");
                return false;
            }

            // Validate role selection
            if (cmbRole.SelectedItem == null)
            {
                ShowError("Please select your role.");
                return false;
            }

            // Validate terms agreement
            if (chkTerms.IsChecked != true)
            {
                ShowError("You must agree to the Terms of Service and Privacy Policy.");
                return false;
            }

            return true;
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
            MessageBox.Show(message, "Registration Error",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void lnkSignIn_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}