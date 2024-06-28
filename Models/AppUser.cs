using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RunTo.Models
{
    public class AppUser : IdentityUser
    {
        [Key]
       
        public int? Pace { get; set; } 
        public int? mileage { get; set; }
        public string ? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }    
        [ForeignKey("Adress")]
        public int? AddreeId { get; set; }
        public Adress? Addresse {  get; set; }
        public ICollection<Race> Races { get; set; }
        public ICollection<Club> Clubs { get; set; }

    }
}
