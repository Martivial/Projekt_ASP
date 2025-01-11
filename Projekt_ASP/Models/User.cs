using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nazwa użytkownika musi mieć od 3 do 100 znaków.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [StringLength(100, ErrorMessage = "Adres email nie może być dłuższy niż 100 znaków.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć od 6 do 100 znaków.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public ICollection<Ad>? Ads { get; set; } // Relacja 1 do wielu, może być null

    }
}