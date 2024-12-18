using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projet_E_commerce.Data;
using Projet_E_commerce.Models;

namespace Projet_E_commerce.Controllers
{
    public class Settings : Controller
    {
        private readonly ApplicationDbContext _context;

        public Settings(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            var model = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password=user.Password
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateUser(String Username ,String Email , String Password ) {

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            if (Username != null)
            {
                user.Username = Username;
            }
            if(Email != null)
            {
                user.Email = Email;
            }
            if(Password != null)
            {
                user.Password = Password;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");


        }




    }
}
