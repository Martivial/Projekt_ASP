using Microsoft.EntityFrameworkCore;


namespace Projekt_ASP.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Reprezentacja tabeli Ads
        public DbSet<Ad> Ads { get; set; }
        public DbSet<User> Users { get; set; }
    }
    
}
