using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Models
{
    public class AdImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] ImageData { get; set; }

        [Required]
        public int AdId { get; set; }

        public Ad Ad { get; set; }
    }
}
