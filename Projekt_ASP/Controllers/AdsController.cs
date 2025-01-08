using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Projekt_ASP.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

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

    [ServiceFilter(typeof(SessionValidationFilter))]
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
        public async Task<IActionResult> Create([Bind("Title,Description,Price,Category,PhoneNumber,Location,VehicleBrand,VehicleModel,VehicleYear,VehicleMileage,PropertyType,PropertyArea,PropertyRooms,JobTitle,JobCompany,JobEmploymentType,ElectronicsType,ElectronicsBrand,ElectronicsModel,ServiceType,ServiceDescription,HomeAndGardenType,HomeAndGardenCondition,FashionType,FashionSize,FashionColor,KidsItemType,KidsAgeRange")] Ad ad, List<IFormFile> images)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"UserId from session: {userIdString}");

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
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(ad);
        }

        // GET: Ads/MyAds
        public IActionResult MyAds()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"UserId from session: {userIdString}");

            if (int.TryParse(userIdString, out int userId))
            {
                var ads = _context.Ads
                    .Where(a => a.UserId == userId)
                    .Include(a => a.Images) // Load images
                    .ToList();
                return View(ads);
            }
            else
            {
                TempData["ErrorMessage"] = "Musisz być zalogowany, aby zobaczyć swoje ogłoszenia.";
                return RedirectToAction("Login", "User");
            }
        }

        // GET: Ads/Edit/5
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

            if (ad.UserId.ToString() != userId)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(ad);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,Description,Price,PhoneNumber,Location,VehicleBrand,VehicleModel,VehicleYear,VehicleMileage,PropertyType,PropertyArea,PropertyRooms,JobTitle,JobCompany,JobEmploymentType,ElectronicsType,ElectronicsBrand,ElectronicsModel,ServiceType,ServiceDescription,HomeAndGardenType,HomeAndGardenCondition,FashionType,FashionSize,FashionColor,KidsItemType,KidsAgeRange,UserId")] Ad ad, List<IFormFile> imageFiles, List<int> imageIds, List<IFormFile> newImages)
        {
            if (id != ad.Id)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"UserId from session: {userId}");
            Console.WriteLine($"UserId from ad: {ad.UserId}");

            if (ad.UserId.ToString() != userId)
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

                    // Update ad properties
                    existingAd.Category = ad.Category;
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

                    // Update specific images if provided
                    if (imageFiles != null && imageIds != null)
                    {
                        for (int i = 0; i < imageFiles.Count; i++)
                        {
                            if (imageFiles[i] != null && imageFiles[i].Length > 0)
                            {
                                var imageId = imageIds[i];
                                var imageToUpdate = existingAd.Images.FirstOrDefault(img => img.Id == imageId);
                                if (imageToUpdate != null && imageToUpdate.Ad.UserId.ToString() == userId)
                                {
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        await imageFiles[i].CopyToAsync(memoryStream);
                                        imageToUpdate.ImageData = memoryStream.ToArray();
                                        _context.Entry(imageToUpdate).State = EntityState.Modified; // Mark image as modified
                                        Console.WriteLine($"Updated image with ID: {imageId}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Image with ID: {imageId} not found or does not belong to the user.");
                                }
                            }
                        }
                    }

                    // Add new images if provided
                    if (newImages != null && newImages.Count > 0)
                    {
                        foreach (var newImage in newImages)
                        {
                            if (newImage.Length > 0)
                            {
                                // Check the size of the new image (e.g., max 5MB)
                                if (newImage.Length > 5 * 1024 * 1024)
                                {
                                    ModelState.AddModelError("", "One of the new images exceeds the maximum allowed size of 5MB.");
                                    return View(ad);
                                }

                                // Check the format of the new image
                                var allowedFormats = new[] { ".jpg", ".jpeg", ".png" };
                                var extension = Path.GetExtension(newImage.FileName).ToLower();
                                if (!allowedFormats.Contains(extension))
                                {
                                    ModelState.AddModelError("", "One of the new images has an invalid format. Allowed formats are .jpg, .jpeg, .png.");
                                    return View(ad);
                                }

                                using (var memoryStream = new MemoryStream())
                                {
                                    await newImage.CopyToAsync(memoryStream);
                                    var adImage = new AdImage
                                    {
                                        ImageData = memoryStream.ToArray(),
                                        AdId = existingAd.Id
                                    };
                                    existingAd.Images.Add(adImage);
                                    Console.WriteLine($"Added new image for ad ID: {existingAd.Id}");
                                }
                            }
                        }
                    }

                    _context.Update(existingAd);
                    await _context.SaveChangesAsync();

                    // Clear file input fields
                    ModelState.Clear();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Concurrency exception: {ex.Message}");
                    if (!AdExists(ad.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirect back to the edit view of the same ad
                return RedirectToAction(nameof(Edit), new { id = ad.Id });
            }

            return View(ad);
        }

        private bool AdExists(int id)
        {
            return _context.Ads.Any(e => e.Id == id);
        }
    


// GET: Ads/CategoryAds
        public IActionResult CategoryAds(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("Index", "Home");
            }

            // Pobranie ogłoszeń na podstawie kategorii
            var ads = _context.Ads
                .Where(a => a.Category == category)
                .Include(a => a.Images) // Załaduj obrazy powiązane z ogłoszeniami
                .ToList();

            // Przekazanie ogłoszeń i kategorii do widoku
            ViewData["Category"] = category;
            return View(ads);
        }
    }
}
