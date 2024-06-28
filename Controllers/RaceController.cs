using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunTo.Data;
using RunTo.Interfaces;
using RunTo.Models;
using RunTo.Repository;
using RunTo.Services;
using RunTo.ViewModel;

namespace RunTo.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _racerepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RaceController(IRaceRepository racerepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _racerepository = racerepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _racerepository.GetAll();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _racerepository.GetByIdAsync(id);
            return View(race);
        }
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var CreatRaceViewModel = new CreateRaceViewModel { AppUserId = curUserId };
            return View(CreatRaceViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            {
                if (ModelState.IsValid)
                {
                    var result = await _photoService.AddPhotoAsync(raceVM.Image);

                    if (result == null || string.IsNullOrEmpty(result.Url?.ToString()))
                    {
                        ModelState.AddModelError("Image", "Photo upload failed!");
                        return View(raceVM);
                    }

                    var race = new Race
                    {
                        Title = raceVM.Title,
                        Description = raceVM.Description,
                        AppUserId=raceVM.AppUserId,
                        Image = result.Url.ToString(),
                        Address = new Adress
                        {
                            Street = raceVM.Address.Street,
                            City = raceVM.Address.City,
                            state = raceVM.Address.state,
                        }
                    };

                    _racerepository.Add(race);
                    return RedirectToAction("Index");
                }


                return View(raceVM);
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _racerepository.GetByIdAsync(id);

            if (race == null)
            {
                return View("Error");
            }

            var raceVM = new EditRaceViewModel
            {
                Id = race.Id, // Ensure to set Id in ViewModel
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory =race.RaceCategory,
            };

            return View(raceVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", raceVM);
            }

            var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(raceVM);
            }

            var race = new Race
            {
                Id = raceVM.Id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = raceVM.AddressId,
                Address = raceVM.Address,
            };

            _racerepository.Update(race);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int Id)
        {
            var raceDetail = await _racerepository.GetByIdAsync(Id);
            if (raceDetail == null) return View("Error");
            return View(raceDetail);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int Id)
        {
            var raceDetail = await _racerepository.GetByIdAsync(Id);
            if (raceDetail == null) return View("Error");
            _racerepository.Delete(raceDetail);
            return RedirectToAction("Index");



        }

    }
}
