using RunTo.Data.Enum;
using RunTo.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunTo.ViewModel
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile  Image { get; set; }
        public string ?URL { get; set; }
        public int?AddressId { get; set; }
        public Adress? Address { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
