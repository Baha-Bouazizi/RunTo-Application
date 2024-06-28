using Microsoft.EntityFrameworkCore;
using RunTo.Data;
using RunTo.Interfaces;
using RunTo.Models;

namespace RunTo.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        public DashboardRepository(ApplicationDbContext context,IHttpContextAccessor contextAccessor) {
            _context=context;
            _httpcontextAccessor=contextAccessor;
        }    
        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser =  _httpcontextAccessor.HttpContext?.User.GetUserId();
            var UserClubs = _context.Clubs.Where(r => r.AppUser.Id == curUser);
            return UserClubs.ToList();
                
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpcontextAccessor.HttpContext?.User.GetUserId();
            var UserRaces = _context.Races.Where(r => r.AppUser.Id == curUser);
            return UserRaces.ToList();
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await  _context.Users.FindAsync(id);
              
        }
       public async Task<AppUser> GetByIdNoTracking(string  id)
        {
            return await _context.Users.Where(U => U.Id == id).AsNoTracking().FirstOrDefaultAsync();
                
        }
        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
          
        }
        public bool Save()
        {
           var saved=_context.SaveChanges();
            return saved > 0 ? true : false;
                
        }
    }
}
