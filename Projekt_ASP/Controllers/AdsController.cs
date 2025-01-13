using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Projekt_ASP.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Projekt_ASP.Controllers
{
    public class SessionValidationFilter : IAsyncActionFilter
    {
        private readonly ApplicationDbContext _context;

        public SessionValidationFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var userId = httpContext.Session.GetString("UserId");
            var userName = httpContext.Session.GetString("UserName");
            var password = httpContext.Session.GetString("Password");

            if (userId == null || userName == null || password == null)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId && u.UserName == userName && u.Password == password);
            if (user == null)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            await next();
        }
    }

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
        [ServiceFilter(typeof(SessionValidationFilter))]  // Zastosowanie filtra tylko dla akcji wymagających logowania
        public async Task<IActionResult> Create([Bind("Title,Description,Price,Category,PhoneNumber,Location,VehicleBrand,VehicleModel,VehicleYear,VehicleMileage,PropertyType,PropertyArea,PropertyRooms,JobTitle,JobCompany,JobEmploymentType,ElectronicsType,ElectronicsBrand,ElectronicsModel,ServiceType,ServiceDescription,HomeAndGardenType,HomeAndGardenCondition,FashionType,FashionSize,FashionColor,KidsItemType,KidsAgeRange")] Ad ad, List<IFormFile> images)
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (ModelState.IsValid)
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    ad.UserId = userId;
                    ad.PostedDate = DateTime.Now;

                    if (images != null && images.Count > 0)
                    {
                        foreach (var image in images.Take(10)) // Limit to 10 images
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await image.CopyToAsync(memoryStream);
                                var adImage = new AdImage
                                {
                                    ImageData = memoryStream.ToArray(),
                                    Ad = ad
                                };
                                ad.Images.Add(adImage);
                            }
                        }
                    }

                    _context.Add(ad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    TempData["ErrorMessage"] = "Nie jesteś zalogowany.";
                    return RedirectToAction("Login", "User");
                }
            }

            return View(ad);
        }

        // GET: Ads/MyAds
        [ServiceFilter(typeof(SessionValidationFilter))]  // Zastosowanie filtra tylko dla akcji wymagających logowania
        public IActionResult MyAds()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName");
            IQueryable<Ad> adsQuery = _context.Ads.Include(a => a.Images);

            if (userName != "admin")
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    adsQuery = adsQuery.Where(a => a.UserId == userId);
                }
                else
                {
                    TempData["ErrorMessage"] = "Musisz być zalogowany, aby zobaczyć swoje ogłoszenia.";
                    return RedirectToAction("Login", "User");
                }
            }

            var ads = adsQuery.ToList();
            return View(ads);
        }

        // GET: Ads/Edit/5
        [ServiceFilter(typeof(SessionValidationFilter))]  // Zastosowanie filtra tylko dla akcji wymagających logowania
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ad = await _context.Ads.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (ad == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            if (ad.UserId.ToString() != userId && userName != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(ad);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SessionValidationFilter))] // Zastosowanie filtra tylko dla akcji wymagających logowania
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Category,Description,Price,PhoneNumber,Location,VehicleBrand,VehicleModel,VehicleYear,VehicleMileage,PropertyType,PropertyArea,PropertyRooms,JobTitle,JobCompany,JobEmploymentType,ElectronicsType,ElectronicsBrand,ElectronicsModel,ServiceType,ServiceDescription,HomeAndGardenType,HomeAndGardenCondition,FashionType,FashionSize,FashionColor,KidsItemType,KidsAgeRange,UserId")] Ad ad,
            List<IFormFile> imageFiles,
            List<int> imageIds,
            List<IFormFile> newImages,
            List<int> deleteImageIds)
        {
            if (id != ad.Id)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            if (ad.UserId.ToString() != userId && userName != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAd = await _context.Ads.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
                    if (existingAd == null)
                    {
                        return NotFound();
                    }

                    // Ensure at least one image remains
                    if (deleteImageIds != null && deleteImageIds.Count >= existingAd.Images.Count)
                    {
                        TempData["ErrorMessage"] = "Musi pozostać co najmniej jedno zdjęcie.";
                        return RedirectToAction("Edit", new { id = ad.Id });
                    }

                    // Delete selected images
                    if (deleteImageIds != null && deleteImageIds.Count > 0)
                    {
                        foreach (var imageId in deleteImageIds)
                        {
                            var imageToDelete = existingAd.Images.FirstOrDefault(img => img.Id == imageId);
                            if (imageToDelete != null)
                            {
                                _context.AdImages.Remove(imageToDelete);
                            }
                        }
                    }

                    // Update existing images
                    foreach (var image in existingAd.Images)
                    {
                        var fileKey = $"imageFiles_{image.Id}";
                        var formFile = Request.Form.Files[fileKey];
                        if (formFile != null && formFile.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                image.ImageData = memoryStream.ToArray();
                            }
                        }
                    }

                    // Add new images
                    if (newImages != null && newImages.Count > 0)
                    {
                        foreach (var newImage in newImages)
                        {
                            if (newImage.Length > 0)
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    await newImage.CopyToAsync(memoryStream);
                                    var adImage = new AdImage
                                    {
                                        ImageData = memoryStream.ToArray(),
                                        AdId = existingAd.Id
                                    };
                                    existingAd.Images.Add(adImage);
                                }
                            }
                        }
                    }

                    // Update ad properties
                    existingAd.Title = ad.Title;
                    existingAd.Description = ad.Description;
                    existingAd.Price = ad.Price;
                    existingAd.PhoneNumber = ad.PhoneNumber;
                    existingAd.Location = ad.Location;
                    existingAd.VehicleBrand = ad.VehicleBrand;
                    existingAd.VehicleModel = ad.VehicleModel;
                    existingAd.VehicleYear = ad.VehicleYear;
                    existingAd.VehicleMileage = ad.VehicleMileage;
                    existingAd.PropertyType = ad.PropertyType;
                    existingAd.PropertyArea = ad.PropertyArea;
                    existingAd.PropertyRooms = ad.PropertyRooms;
                    existingAd.JobTitle = ad.JobTitle;
                    existingAd.JobCompany = ad.JobCompany;
                    existingAd.JobEmploymentType = ad.JobEmploymentType;
                    existingAd.ElectronicsType = ad.ElectronicsType;
                    existingAd.ElectronicsBrand = ad.ElectronicsBrand;
                    existingAd.ElectronicsModel = ad.ElectronicsModel;
                    existingAd.ServiceType = ad.ServiceType;
                    existingAd.ServiceDescription = ad.ServiceDescription;
                    existingAd.HomeAndGardenType = ad.HomeAndGardenType;
                    existingAd.HomeAndGardenCondition = ad.HomeAndGardenCondition;
                    existingAd.FashionType = ad.FashionType;
                    existingAd.FashionSize = ad.FashionSize;
                    existingAd.FashionColor = ad.FashionColor;
                    existingAd.KidsItemType = ad.KidsItemType;
                    existingAd.KidsAgeRange = ad.KidsAgeRange;

                    _context.Update(existingAd);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Zapisano zmiany!";
                    return RedirectToAction("Edit", new { id = ad.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdExists(ad.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction("Edit", new { id = ad.Id });
        }

        private bool AdExists(int id)
        {
            return _context.Ads.Any(e => e.Id == id);
        }

        // DELETE: Ads/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SessionValidationFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ad = await _context.Ads.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            var userName = HttpContext.Session.GetString("UserName");

            if (ad == null)
            {
                return NotFound();
            }

            if (userName != "admin" && ad.UserId.ToString() != HttpContext.Session.GetString("UserId"))
            {
                TempData["ErrorMessage"] = "Nie masz uprawnień do usunięcia tego ogłoszenia.";
                return RedirectToAction("Index", "Home");
            }

            if (ad.Images != null && ad.Images.Any())
            {
                _context.AdImages.RemoveRange(ad.Images);
            }

            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ogłoszenie zostało usunięte.";
            return RedirectToAction("MyAds");
        }



        // **Dostęp publiczny** do kategorii ogłoszeń
        [AllowAnonymous]
        public IActionResult CategoryAds(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("Index", "Home");
            }

            var ads = _context.Ads
                .Where(a => a.Category == category)
                .Include(a => a.Images)
                .ToList();

            ViewData["Category"] = category;
            return View(ads);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ad = await _context.Ads
                .Include(a => a.Images) // Załaduj obrazy związane z ogłoszeniem
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ad == null)
            {
                return NotFound();
            }

            return View(ad); // Przekaż ogłoszenie do widoku
        }
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                TempData["Message"] = "Wpisz frazę, aby wyszukać ogłoszenia.";
                return RedirectToAction("Index", "Home");
            }

            var ads = await _context.Ads
                .Include(a => a.Images)
                .Where(a => a.Title.ToUpper().Contains(query.ToUpper())) // SQL-compatible
                .ToListAsync();

            ViewData["Query"] = query;
            return View(ads);
        }



    }
}