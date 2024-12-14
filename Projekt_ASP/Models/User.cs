using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        
        [StringLength(100)]
        public required string UserName { get; set; }

        
        [StringLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        
        [StringLength(100)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

    }
}