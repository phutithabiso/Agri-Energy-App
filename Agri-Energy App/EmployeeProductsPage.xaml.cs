using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class EmployeeProductsPage : Page, INotifyPropertyChanged
    {
        private readonly User _currentUser;
        private readonly AppDbContext _dbContext;

        public List<string> CategoryOptions { get; set; }
        private string _selectedCategory = "All Categories";
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilterProducts(null, null); // Auto-filter when category changes
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public EmployeeProductsPage(User user, AppDbContext dbContext)
        {
            InitializeComponent();
            _currentUser = user;
            _dbContext = dbContext;

            CategoryOptions = new List<string>
            {
                "All Categories", "Vegetables", "Fruits", "Grains", "Dairy"
            };

            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                cmbFarmers.ItemsSource = _dbContext.Users
                    .Where(u => u.Role == "Farmer")
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ToList();
                cmbFarmers.SelectedIndex = -1;

                productsGrid.ItemsSource = _dbContext.Products
                    .Include(p => p.Farmer)
                    .OrderBy(p => p.Farmer.LastName)
                    .ThenBy(p => p.Farmer.FirstName)
                    .ThenBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterProducts(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = _dbContext.Products
                    .Include(p => p.Farmer)
                    .AsQueryable();

                if (cmbFarmers.SelectedValue is int farmerId)
                {
                    query = query.Where(p => p.FarmerId == farmerId);
                }

                if (!string.IsNullOrWhiteSpace(SelectedCategory) && SelectedCategory != "All Categories")
                {
                    query = query.Where(p => p.Category == SelectedCategory);
                }

                if (dpFromDate.SelectedDate.HasValue && dpToDate.SelectedDate.HasValue)
                {
                    var from = dpFromDate.SelectedDate.Value.Date;
                    var to = dpToDate.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(p => p.ProductionDate >= from && p.ProductionDate <= to);
                }
                else if (dpFromDate.SelectedDate.HasValue)
                {
                    var from = dpFromDate.SelectedDate.Value.Date;
                    query = query.Where(p => p.ProductionDate >= from);
                }
                else if (dpToDate.SelectedDate.HasValue)
                {
                    var to = dpToDate.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(p => p.ProductionDate <= to);
                }

                productsGrid.ItemsSource = query
                    .OrderBy(p => p.Farmer.LastName)
                    .ThenBy(p => p.Farmer.FirstName)
                    .ThenBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            cmbFarmers.SelectedIndex = -1;
            SelectedCategory = "All Categories";
            dpFromDate.SelectedDate = null;
            dpToDate.SelectedDate = null;

            productsGrid.ItemsSource = _dbContext.Products
                .Include(p => p.Farmer)
                .OrderBy(p => p.Farmer.LastName)
                .ThenBy(p => p.Farmer.FirstName)
                .ThenBy(p => p.Name)
                .ToList();

            cmbCategories.SelectedItem = SelectedCategory;
        }
    }
}
