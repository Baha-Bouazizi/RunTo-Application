namespace RunTo.ViewModel
{
    public class EditProfileViewModel

    {
        public string  ?Id { get; set; }
        public int? Pace { get; set; }
        public int? mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public IFormFile? Image { get; set; }
    }
}