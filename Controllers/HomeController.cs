using Microsoft.AspNetCore.Mvc;
using Projet_E_commerce.Data;
using Projet_E_commerce.Models;


namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("./Views/Home/Index.cshtml");
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                ViewBag.Message = "User registered successfully!";
                return View("./Views/Home/Login.cshtml");
            }
            return View("./Views/Home/Signup.cshtml");
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null && password.Equals(user.Password)) 
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                ViewBag.Message = "Login successful!";
                return RedirectToAction("Index", "Store");
            }
            ViewBag.Error = "Invalid username or password.";
            return View();
        }
    }
}
