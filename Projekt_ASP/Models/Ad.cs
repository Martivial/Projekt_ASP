using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Models
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string Category { get; set; } // e.g., Car, Motorcycle, Truck, Real Estate, Job, Electronics, Services, Home and Garden, Fashion, For Kids

        // Motoryzacja
        public string? VehicleBrand { get; set; }
        public string? VehicleModel { get; set; }
        public int? VehicleYear { get; set; }
        public int? VehicleMileage { get; set; }

        // Nieruchomości
        public string? PropertyType { get; set; }
        public int? PropertyArea { get; set; } // in square meters
        public int? PropertyRooms { get; set; }

        // Praca
        public string? JobTitle { get; set; }
        public string? JobCompany { get; set; }
        public string? JobEmploymentType { get; set; } // e.g., Full-time, Part-time, Contract

        // Elektronika
        public string? ElectronicsType { get; set; }
        public string? ElectronicsBrand { get; set; }
        public string? ElectronicsModel { get; set; }

        // Usługi
        public string? ServiceType { get; set; }
        public string? ServiceDescription { get; set; }

        // Dom i ogród
        public string? HomeAndGardenType { get; set; }
        public string? HomeAndGardenCondition { get; set; } // e.g., New, Used

        // Moda
        public string? FashionType { get; set; }
        public string? FashionSize { get; set; }
        public string? FashionColor { get; set; }

        // Dla dzieci
        public string? KidsItemType { get; set; }
        public string? KidsAgeRange { get; set; }

        // Pola uniwersalne
        [Required]
        public decimal? Price { get; set; }

        [Required]
        public string Location { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public string? PhoneNumber { get; set; }

        // Collection of images
        public ICollection<AdImage> Images { get; set; } = new List<AdImage>();

        // Pole łączące ogłoszenie z użytkownikiem
        [Required]
        public int UserId { get; set; }
    }
}