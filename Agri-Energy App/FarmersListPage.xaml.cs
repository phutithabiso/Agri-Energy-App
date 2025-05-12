using System.Linq;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_App
{
    public partial class FarmersListPage : Page
    {
        private readonly AppDbContext _dbContext;

        public FarmersListPage(User currentUser, AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadFarmers();
        }

        private void LoadFarmers()
        {
            var farmers = _dbContext.Users
                .Where(u => u.Role == "Farmer")
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            farmersGrid.ItemsSource = farmers;
        }
    }
}