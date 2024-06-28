using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunTo.Interfaces;
using RunTo.Models;
using RunTo.Services;
using RunTo.ViewModel;


namespace RunTo.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var UserViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    mileage = user.mileage,
                    ProfileImageUrl = user.ProfileImageUrl,
                };
                result.Add(UserViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound(); // Retourne une erreur 404 si l'utilisateur n'est pas trouvé
            }

            var userDetailViewModel = new UserDetailViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                mileage = user.mileage,
                ProfileImageUrl = user.ProfileImageUrl // Assurez-vous que ProfileImageUrl est correctement défini
            };

            return View(userDetailViewModel); // Renvoie la vue Detail avec le modèle UserDetailViewModel
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            var editVM = new EditProfileViewModel()
            {
                City = user.City,
                State = user.State,
                Pace = user.Pace,
                mileage = user.mileage,
                ProfileImageUrl = user.ProfileImageUrl,
            };

            return View(editVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View(editVM);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            // Vérifie si l'utilisateur actuel correspond à l'utilisateur dont le profil est en cours d'édition
            if (user.Id != editVM.Id) // Assurez-vous que EditProfileViewModel contient une propriété Id
            {
                return Forbid(); // Retourne un statut 403 si l'utilisateur n'est pas autorisé à éditer ce profil
            }

            if (editVM.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View(editVM);
                }

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                user.ProfileImageUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageUrl;

                await _userManager.UpdateAsync(user);

                // Redirige vers la vue de détail pour voir le profil mis à jour
                return RedirectToAction("Detail", new { id = user.Id });
            }

            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.mileage = editVM.mileage;

            await _userManager.UpdateAsync(user);

            // Redirige vers la vue de détail pour voir le profil mis à jour
            return RedirectToAction("Detail", new { id = user.Id });
        }

    }
}
