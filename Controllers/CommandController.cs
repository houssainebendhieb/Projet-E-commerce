using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet_E_commerce.Data;

namespace Projet_E_commerce.Controllers
{
    public class CommandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommandController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Afficher les commandes de l'utilisateur
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Récupérer les commandes de l'utilisateur, triées du plus récent au plus ancien
            var orders = _context.Commande
                .Where(c => c.UserId == userId.Value)
                .OrderByDescending(c => c.DateCommande)  // Tri par date (plus récent en premier)
                .ToList();
            
            return View(orders);
        }
    }
}
