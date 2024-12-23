using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projekt_ASP.Models;
using System.Threading.Tasks;

namespace Projekt_ASP.Controllers
{
    public class AdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ads/Create
        public IActionResult Create()
        {
            return View();
        }
        //ADS CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Price,Category,Image,PhoneNumber,Location")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                // Pobieranie identyfikatora zalogowanego użytkownika z sesji
                var userId = HttpContext.Session.GetString("UserId");
                if (userId != null)
                {
                    ad.UserId = int.Parse(userId);  // Przypisanie użytkownika do ogłoszenia
                    ad.CreatedAt = DateTime.Now;  // Ustawienie daty utworzenia ogłoszenia

                    _context.Add(ad);  // Dodanie ogłoszenia do kontekstu
                    await _context.SaveChangesAsync();  // Zapisanie zmian w bazie danych

                    // Przekierowanie na stronę Index po dodaniu ogłoszenia
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Nie jesteś zalogowany.";
                    return RedirectToAction("Login", "User");
                }
            }
            return View(ad);
        }

        // GET: Ads/Index
        public IActionResult Index()
        {
            // Pobranie ogłoszeń posortowanych od najnowszych
            var ads = _context.Ads.OrderByDescending(a => a.CreatedAt).ToList();
            return View(ads);  // Przekazanie ogłoszeń do widoku
        }
        public IActionResult MyAds()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                // Pobranie ogłoszeń dla zalogowanego użytkownika
                var ads = _context.Ads.Where(a => a.UserId == int.Parse(userId)).ToList();
                return View(ads);
            }
            else
            {
                TempData["ErrorMessage"] = "Musisz być zalogowany, aby zobaczyć swoje ogłoszenia.";
                return RedirectToAction("Login", "User");
            }
        }
        public IActionResult CategoryAds(string category)
        {
            // Pobieramy ogłoszenia z wybranej kategorii, posortowane od najnowszego
            var ads = _context.Ads.Where(a => a.Category == category).OrderByDescending(a => a.CreatedAt).ToList();

            // Przekazujemy nazwę kategorii do widoku (aby wyświetlić ją na stronie)
            ViewData["CategoryName"] = category;

            return View(ads);  // Zwracamy ogłoszenia do widoku
        }
    }
}
