using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunTo.Data;
using RunTo.Interfaces;
using RunTo.Models;
using RunTo.ViewModel;

namespace RunTo.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly IPhotoService _photoService;

     
        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpcontextAccessor, IPhotoService photoService)
        {

            _dashboardRepository = dashboardRepository;
            _httpcontextAccessor = httpcontextAccessor;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViwModel = new DashboardViewModel
            {
                Races = userRaces,
                Clubs = userClubs,
            };
            return View(dashboardViwModel);
        }
        
    }
}
