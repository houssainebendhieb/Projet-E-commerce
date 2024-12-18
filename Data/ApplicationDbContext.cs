using Microsoft.EntityFrameworkCore;
using Projet_E_commerce.Models;

namespace Projet_E_commerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Panier> Panier { get; set; }
        public DbSet<Commande> Commande { get; set; }
    }
}
