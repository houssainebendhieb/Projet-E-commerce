using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Projet_E_commerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(255)] // Chemin ou URL de l'image
        public string ImageUrl { get; set; }
    }
}
