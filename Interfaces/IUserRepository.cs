using RunTo.Models;

namespace RunTo.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetById(string  id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(string  id);
        bool Save();
    }
}
