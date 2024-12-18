using System.ComponentModel.DataAnnotations;

namespace Projet_E_commerce.Models
{
    public class Commande
    {
        [Key]
        public int Id { get; set; } // Numéro de commande généré automatiquement

        public int UserId { get; set; } // ID de l'utilisateur
        public decimal TotalPrice { get; set; } // Prix total de l'achat
        public DateTime DateCommande { get; set; } // Date de la commande

        public string PaymentMethod { get; set; } // Méthode de paiement (Visa, MasterCard, etc.)

        // Relations
        public User User { get; set; } // L'utilisateur associé
    }
}
