using Microsoft.EntityFrameworkCore;
using RunTo.Data;
using RunTo.Interfaces;
using RunTo.Models;

namespace RunTo.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();


        }

        public async Task<AppUser> GetById(string  id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Save()
        {
          var result=_context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
