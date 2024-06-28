using RunTo.Data;
using RunTo.Interfaces;
using RunTo.Models;
using Microsoft.EntityFrameworkCore;

namespace RunTo.Repository
{
    public class RaceRepository :IRaceRepository
    {
        private readonly ApplicationDbContext _context;
        public RaceRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public bool Add(Race race)
        {
           _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
           _context.Remove(race);
            return Save();  
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
           return await _context.Races.ToListAsync();
                
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            return await _context.Races.Where(c=>c.Address.City.Contains(city)).ToListAsync();

        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await  _context.Races.Include(c=>c.Address).FirstOrDefaultAsync(c=>c.Id == id);

        }

        public bool Save()
        {
            var  save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool Update(Race race)
        {
           _context.Update(race);
            return Save();
        }
    }
}
