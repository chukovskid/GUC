using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entit) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams); // PagedList e zamena za IEnumerable<T>
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id); // S11
         Task<Photo> GetMainPhotoForUser(int id); // 113
    }
}