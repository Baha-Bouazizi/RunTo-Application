using Microsoft.AspNetCore.Mvc;
using RunTo.Interfaces;
using RunTo.Models;
using RunTo.Repository;
using RunTo.ViewModel;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RunTo.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubrepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        public ClubController(IClubRepository clubrepository, IPhotoService photoService, IHttpContextAccessor httpcontextAccessor)
        {
            _clubrepository = clubrepository;
            _photoService = photoService;
            _httpcontextAccessor = httpcontextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubrepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubrepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var curUserId = _httpcontextAccessor.HttpContext?.User.GetUserId();
            var CreateClubViewModel=new CreateClubViewModel { AppUserId=curUserId};
            return View(CreateClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                if (result == null || string.IsNullOrEmpty(result.Url?.ToString()))
                {
                    ModelState.AddModelError("Image", "Photo upload failed!");
                    return View(clubVM);
                }

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId=clubVM.AppUserId,
                    Address = new Adress
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        state = clubVM.Address.state,
                    }
                };

                _clubrepository.Add(club);
                return RedirectToAction("Index");
            }

            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubrepository.GetByIdAsync(id);

            if (club == null)
            {
                return View("Error");
            }

            var clubVM = new EditClubViewModel
            {
                Id = club.Id,
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(clubVM);
            }

            var club = new Club
            {
                Id = clubVM.Id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = clubVM.AddressId,
                Address = clubVM.Address,
                ClubCategory = clubVM.ClubCategory
            };

            _clubrepository.Update(club);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int Id)
        {
            var clubDetail= await _clubrepository.GetByIdAsync(Id);
            if (clubDetail == null) return View("Error");
            return  View(clubDetail);
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int Id)
        {
            var clubDetail = await _clubrepository.GetByIdAsync(Id);
            if (clubDetail == null) return View("Error");
            _clubrepository.Delete(clubDetail);
            return RedirectToAction("Index");

        

        }

    }
}