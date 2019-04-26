using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Garlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Garlow.API.Data
{
    public class GarlowRepository : IGarlowRepository
    {
        private readonly DataContext _context;

        public GarlowRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T: class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T: class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(u => u.Photos).ToListAsync();
        }

        public async Task<User> GetUser(int userId)
        {
            return await _context.Users.Include(u => u.Photos).Include(u => u.Locations).SingleOrDefaultAsync(u => u.Id == userId);
        }
        
        public async Task<Photo> GetPhoto(int photoId)
        {
            return await _context.Photos.SingleOrDefaultAsync(p => p.Id == photoId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(p => p.UserId == userId).SingleOrDefaultAsync(p => p.IsMain);
        }

        public async Task<IEnumerable<Location>> GetLocationsForUser(int userId)
        {
            return await _context.Locations.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<Location> GetLocation(int locationId)
        {
            return await _context.Locations.SingleOrDefaultAsync(l => l.Id == locationId);
        }
    }
}