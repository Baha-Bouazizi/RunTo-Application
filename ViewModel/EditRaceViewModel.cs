﻿using RunTo.Data.Enum;
using RunTo.Models;

namespace RunTo.ViewModel
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public int AddressId { get; set; }
        public Adress? Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
       
    }
}
