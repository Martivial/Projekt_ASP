using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Models
{
    public class Ad
    {
        
  
        public int Id { get; set; }

        [Display(Name ="Tytuł ogłoszenia")]
        public required string Title { get; set; }

        [Display(Name ="Opis")]
        public required string Description { get; set; }

        [Display(Name ="Cena")]
        public decimal Price { get; set; }

        [Display(Name ="Kategoria")]
        public required string Category { get; set; }


        public required string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
