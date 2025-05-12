using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly AppDbContext _dbContext = new AppDbContext();
        private readonly User _user;

        public ChangePasswordWindow(User user)
        {
            InitializeComponent();
            _user = user;
            Closing += (s, e) => _dbContext.Dispose();
        }

        private async void Change_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Password))
            {
                MessageBox.Show("Please enter your current password.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!PasswordHasher.VerifyPassword(txtCurrentPassword.Password, _user.PasswordHash))
            {
                MessageBox.Show("Current password is incorrect.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtNewPassword.Password) ||
                txtNewPassword.Password.Length < 8)
            {
                MessageBox.Show("New password must be at least 8 characters.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtNewPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("New passwords do not match.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _user.PasswordHash = PasswordHasher.HashPassword(txtNewPassword.Password);
                _dbContext.Entry(_user).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                MessageBox.Show("Password changed successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing password: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}