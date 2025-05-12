using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri_Energy_App
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; } = DateTime.Now;
        public string FullName => $"{FirstName} {LastName}";

        public Product Product
        {
            get => default;
            set
            {
            }
        }
    }
}
