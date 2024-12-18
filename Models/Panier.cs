
namespace Projet_E_commerce.Models
{
    public class Panier
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public int ProductId { get; set; } 
        public int Quantity { get; set; } 
        // Relations
        public Product Product { get; set; }
    }

}
