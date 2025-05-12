using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class PasswordResetWindow : Window
    {
        private readonly AppDbContext _dbContext = new AppDbContext();
        private string? _email;
        private string? _verificationCode;
        private PasswordResetStep _currentStep = PasswordResetStep.EmailEntry;

        public PasswordResetWindow(string? email = null)
        {
            InitializeComponent();
            _dbContext.Database.EnsureCreated();

            _email = email;
            if (!string.IsNullOrEmpty(_email))
            {
                txtEmail.Text = _email;
                AdvanceToVerificationCodeStep();
            }
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentStep)
            {
                case PasswordResetStep.EmailEntry:
                    HandleEmailSubmission();
                    break;
                case PasswordResetStep.CodeVerification:
                    VerifyCode();
                    break;
                case PasswordResetStep.PasswordChange:
                    ChangePassword();
                    break;
            }
        }

        private void HandleEmailSubmission()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                ShowStatus("Please enter a valid email address.");
                return;
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Email == txtEmail.Text);
            if (user == null)
            {
                ShowStatus("No account found with this email address.");
                return;
            }

            _email = txtEmail.Text;

            // In a real app, you would send an actual email here
            _verificationCode = GenerateVerificationCode();
            Console.WriteLine($"DEBUG: Verification code for {_email} is {_verificationCode}"); 

            ShowStatus($"A verification code has been sent to {_email}. Enter it above.");
            AdvanceToVerificationCodeStep();
        }

        private void VerifyCode()
        {
            if (string.IsNullOrWhiteSpace(txtVerificationCode.Text) ||
                txtVerificationCode.Text != _verificationCode)
            {
                ShowStatus("Invalid verification code. Please try again.");
                return;
            }

            AdvanceToPasswordChangeStep();
        }

        private void ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Password) ||
                txtNewPassword.Password.Length < 8)
            {
                ShowStatus("Password must be at least 8 characters long.");
                return;
            }

            if (!Regex.IsMatch(txtNewPassword.Password, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
            {
                ShowStatus("Password must contain at least one letter and one number.");
                return;
            }

            if (txtNewPassword.Password != txtConfirmPassword.Password)
            {
                ShowStatus("Passwords do not match.");
                return;
            }

            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == _email);
                if (user != null)
                {
                    user.PasswordHash = PasswordHasher.HashPassword(txtNewPassword.Password);
                    _dbContext.SaveChanges();

                    MessageBox.Show("Your password has been successfully reset.", "Password Reset",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Error resetting password: {ex.Message}");
            }
        }

        private void AdvanceToVerificationCodeStep()
        {
            _currentStep = PasswordResetStep.CodeVerification;
            emailPanel.Visibility = Visibility.Visible;
            codePanel.Visibility = Visibility.Visible;
            passwordPanel.Visibility = Visibility.Collapsed;
            confirmPasswordPanel.Visibility = Visibility.Collapsed;
            btnAction.Content = "Verify Code";
            txtVerificationCode.Focus();
        }

        private void AdvanceToPasswordChangeStep()
        {
            _currentStep = PasswordResetStep.PasswordChange;
            emailPanel.Visibility = Visibility.Visible;
            codePanel.Visibility = Visibility.Visible;
            passwordPanel.Visibility = Visibility.Visible;
            confirmPasswordPanel.Visibility = Visibility.Visible;
            btnAction.Content = "Reset Password";
            txtNewPassword.Focus();
            ShowStatus("Now enter your new password.");
        }

        private string GenerateVerificationCode()
        {
            // In production, use a more secure random number generator
            var random = new Random();
            return random.Next(100000, 999999).ToString();
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

        private void ShowStatus(string message)
        {
            txtStatus.Text = message;
        }

        private enum PasswordResetStep
        {
            EmailEntry,
            CodeVerification,
            PasswordChange
        }
    }
}