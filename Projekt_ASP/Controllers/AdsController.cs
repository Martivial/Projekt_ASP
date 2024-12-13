using Microsoft.AspNetCore.Mvc;
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

        // POST: Ads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Price,Category")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                ad.CreatedAt = DateTime.Now;  // Ustawienie daty utworzenia ogłoszenia
                _context.Add(ad);  // Dodanie ogłoszenia do kontekstu
                await _context.SaveChangesAsync();  // Zapisanie zmian w bazie danych

                // Przekierowanie na stronę Index po dodaniu ogłoszenia
                return RedirectToAction(nameof(Index));  // Przekierowanie na stronę Index
            }
            return View(ad);  // Jeśli ModelState nie jest ważny, zwróci formularz
        }

        // GET: Ads/Index
        public IActionResult Index()
        {
            var ads = _context.Ads.ToList();  // Pobieranie wszystkich ogłoszeń
            return View(ads);  // Przekazanie ogłoszeń do widoku
        }

    }
}
