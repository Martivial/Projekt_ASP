using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Models
{
    public class Ad
    {

        
        public int Id { get; set; }

        [Display(Name ="Tytuł ogłoszenia")]
        public string Title { get; set; }

        [Display(Name ="Opis")]
        public string Description { get; set; }

        [Display(Name ="Cena")]
        public decimal Price { get; set; }

        [Display(Name ="Kategoria")]
        public string Category { get; set; }


        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
