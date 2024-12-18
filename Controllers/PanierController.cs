using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_E_commerce.Data;
using Projet_E_commerce.Models;

namespace Projet_E_commerce.Controllers
{
    public class PanierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PanierController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Afficher les produits dans le panier
        public IActionResult Index()
        {
           
              var userId = HttpContext.Session.GetInt32("UserId");
              if (userId == null)
              {
                  return RedirectToAction("Login", "Home");
              }

            var cartItems = _context.Panier
             .Where(c => c.UserId == userId)
             .Include(c => c.Product) // Inclure les détails du produit associé
             .ToList();
            /*  List<Panier> paniers = new List<Panier>();
              foreach(var a in cartItems)
          {
              var produit = _context.Products.Where(c => a.ProductId == c.Id);
              Panier panier = new Panier();

          }*/

            return View(cartItems);
            
        }

  
        [HttpPost]
        public IActionResult Add(int productId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Vérifier si le produit est déjà dans le panier
            var existingCartItem = _context.Panier
                .FirstOrDefault(ci => ci.UserId == userId && ci.ProductId == productId);

            if (existingCartItem != null)
            {
                // Si le produit est déjà dans le panier, mettre à jour la quantité
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Sinon, ajouter un nouvel élément au panier
                var cartItem = new Models.Panier
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.Panier.Add(cartItem);
            }
            _context.SaveChanges();
            return View("");
        }
      
        [HttpPost]
        public IActionResult Edit(int cartItemId, int newQuantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Récupérer l'article du panier
            var cartItem = _context.Panier
                .FirstOrDefault(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Produit non trouvé dans le panier.");
            }

            if (newQuantity <= 0)
            {
                // Si la quantité est inférieure ou égale à 0, supprimer l'article
                _context.Panier.Remove(cartItem);
            }
            else
            {
                // Sinon, mettre à jour la quantité
                cartItem.Quantity = newQuantity;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Delete(int panierId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }


            var cartItem = _context.Panier
                .FirstOrDefault(c => c.Id == panierId && c.UserId == userId.Value);

            if (cartItem == null)
            {
                return NotFound("L'article du panier n'existe pas ou vous n'avez pas les permissions.");
            }

            _context.Panier.Remove(cartItem);
            _context.SaveChanges();

            TempData["Message"] = "Produit supprimé du panier avec succès.";
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var cartItems = _context.Panier
                .Where(c => c.UserId == userId.Value)
                .Include(c => c.Product)
                .ToList();

            var totalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity);

            ViewBag.TotalPrice = totalPrice;
            return View();
        }


        [HttpPost]
        public IActionResult Pay(string PaymentMethod, string CardNumber, string ExpirationDate, string CVV)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            // Calculer le prix total
            var cartItems = _context.Panier
                .Where(c => c.UserId == userId.Value)
                .Include(c => c.Product)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Votre panier est vide.";
                return RedirectToAction("Index");
            }

            var totalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity);

            // Créer une nouvelle commande
            var newCommande = new Commande
            {
                UserId = userId.Value,
                TotalPrice = totalPrice,
                DateCommande = DateTime.Now,
                PaymentMethod = PaymentMethod
            };

            _context.Commande.Add(newCommande);

            // Vider le panier
            _context.Panier.RemoveRange(cartItems);

            _context.SaveChanges();

            TempData["Success"] = "Paiement effectué avec succès. Votre commande a été enregistrée.";
            return RedirectToAction("Confirmation", new { id = newCommande.Id });
        }

        public IActionResult Confirmation(int id)
        {
            var commande = _context.Commande.FirstOrDefault(c => c.Id == id);
            if (commande == null)
            {
                return NotFound("Commande introuvable.");
            }

            return View(commande);
        }


    }
}
