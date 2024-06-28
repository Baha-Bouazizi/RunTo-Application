using RunTo.Data.Enum;
using RunTo.Models;

namespace RunTo.ViewModel
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Adress Address { get; set; }
        public IFormFile? Image { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public string AppUserId { get; set; }
    }
}
