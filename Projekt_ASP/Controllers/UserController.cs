using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Projekt_ASP.Models;
using System.Linq;

namespace Projekt_ASP.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: User/Index
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public IActionResult Login(string identifier, string password)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Sprawdzamy, czy użytkownik istnieje w bazie danych
            var user = _context.Users.FirstOrDefault(u => u.Email == identifier || u.UserName == identifier);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
            {
                TempData["ErrorMessage"] = "Nie znaleziono takiego użytkownika.";
                return RedirectToAction("Index");
            }

            // Dodajemy użytkownika do sesji
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.UserName);

            return RedirectToAction("Index", "Home");
        }


        // GET: User/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: User/Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Sprawdzamy, czy użytkownik już istnieje w bazie danych
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Użytkownik z takim adresem email już istnieje.";
                return RedirectToAction("Register");
            }

            if (ModelState.IsValid)
            {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View(user);
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        // Inne akcje kontrolera...
    }
}