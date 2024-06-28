using Microsoft.EntityFrameworkCore;
using RunTo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace RunTo.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base (options)
        {

        }
        public DbSet<Race> Races { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet <Club> Clubs { get; set; }
        public object Race { get; internal set; }
        public object Cities { get; internal set; }
    }
}
