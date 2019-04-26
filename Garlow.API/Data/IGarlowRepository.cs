using System.Collections.Generic;
using System.Threading.Tasks;
using Garlow.API.Models;

namespace Garlow.API.Data
{
    public interface IGarlowRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int userId);
        Task<Photo> GetPhoto(int photoId);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<IEnumerable<Location>> GetLocationsForUser(int userId);
        Task<Location> GetLocation(int locationId);
    }
}