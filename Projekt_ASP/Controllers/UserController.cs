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

            var user = _context.Users.FirstOrDefault(u => u.Email == identifier || u.UserName == identifier);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
            {
                TempData["ErrorMessage"] = "Nie znaleziono takiego użytkownika.";
                return RedirectToAction("Index");
            }

            // Set session variables
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("Password", user.Password);



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

            // Logowanie danych użytkownika
            Console.WriteLine($"UserName: {user.UserName}, Email: {user.Email}");

            // Sprawdzenie, czy użytkownik z takim adresem email już istnieje
            var existingUserByEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUserByEmail != null)
            {
                TempData["ErrorMessage"] = "Użytkownik z takim adresem email już istnieje.";
                return RedirectToAction("Register");
            }

            // Sprawdzenie, czy użytkownik z taką nazwą użytkownika już istnieje
            var existingUserByUserName = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (existingUserByUserName != null)
            {
                TempData["ErrorMessage"] = "Użytkownik z taką nazwą użytkownika już istnieje.";
                return RedirectToAction("Register");
            }

            if (ModelState.IsValid)
            {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _context.Users.Add(user);

                try
                {
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Rejestracja zakończona sukcesem!";
                    return RedirectToAction("Index", "User"); // Przekierowanie do widoku Index kontrolera User
                }
                catch (Exception ex)
                {
                    // Logowanie wyjątku
                    Console.WriteLine($"Exception: {ex.Message}");
                    TempData["ErrorMessage"] = "Wystąpił błąd podczas zapisywania danych.";
                }
            }
            else
            {
                // Logowanie błędów walidacji
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }

            return View(user);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
       
    }
}