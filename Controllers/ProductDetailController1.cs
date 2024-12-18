using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet_E_commerce.Data;

namespace Projet_E_commerce.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id  == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
            
        }

        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToPanier(int productId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var existingCartItem = _context.Panier
                .FirstOrDefault(ci => ci.UserId == userId && ci.ProductId == productId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new Models.Panier
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.Panier.Add(cartItem);
            }

            _context.SaveChanges();
            return Redirect("https://localhost:7117/ProductDetail/Details/"+productId);
        }
    }
}
