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
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć od 6 do 100 znaków, zawierać dużą literę, cyfrę i znak specjalny.")]
        [DataType(DataType.Password)]
        [CustomValidation(typeof(User), nameof(ValidatePassword))]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane.")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public ICollection<Ad>? Ads { get; set; } // Relacja 1 do wielu, może być null

        public static ValidationResult? ValidatePassword(string password, ValidationContext context)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Hasło jest wymagane.");
            }

            if (password.Length < 6)
            {
                return new ValidationResult("Hasło musi mieć od 6 do 100 znaków, zawierać dużą literę, cyfrę i znak specjalny");
            }

            if (!password.Any(char.IsUpper))
            {
                return new ValidationResult("Hasło musi zawierać co najmniej jedną dużą literę.");
            }

            if (!password.Any(char.IsDigit))
            {
                return new ValidationResult("Hasło musi zawierać co najmniej jedną cyfrę.");
            }

            if (!password.Any(ch => "!@#$%^&*()_+[]{}|;:,.<>?".Contains(ch)))
            {
                return new ValidationResult("Hasło musi zawierać co najmniej jeden znak specjalny.");
            }

            return ValidationResult.Success;
        }
    }
}