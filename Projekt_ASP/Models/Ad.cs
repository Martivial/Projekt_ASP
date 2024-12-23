using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Models
{
    public class Ad
    {


        public int Id { get; set; }

        [Display(Name = "Tytuł ogłoszenia")]
        public required string Title { get; set; }

        [Display(Name = "Opis")]
        public required string Description { get; set; }

        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Display(Name = "Kategoria")]
        public required string Category { get; set; }

        public int UserId { get; set; } //foreign key do uzytkownika
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Zdjęcie")]
        public byte[]? Image { get; set; }

        [Display(Name = "Numer telefonu")]
        [Phone(ErrorMessage = "Podaj poprawny numer telefonu")]
        public required string PhoneNumber { get; set; }

        [Display(Name = "Lokalizacja")]
        [StringLength(100, ErrorMessage = "Lokalizacja nie może przekraczać 100 znaków.")]
        public required string Location { get; set; }

    }
}
