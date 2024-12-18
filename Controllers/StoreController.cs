using Microsoft.AspNetCore.Mvc;
using Projet_E_commerce.Data;

namespace Projet_E_commerce.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }
       public IActionResult Index(decimal? minPrice, decimal? maxPrice, string category)
        {

      
            var products = _context.Products.AsQueryable();
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }
            ViewBag.Categories = _context.Products
                                          .Select(p => p.Category)
                                          .Distinct()
                                          .ToList();
            return View(products.ToList());
        }
    }
}
