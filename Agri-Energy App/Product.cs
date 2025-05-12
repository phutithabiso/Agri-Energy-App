using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri_Energy_App
{
   public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public DateTime ProductionDate { get; set; }

        public string Description { get; set; }

        [Required]
        public int FarmerId { get; set; }

        // Navigation property
        public User Farmer { get; set; }
    }
}
