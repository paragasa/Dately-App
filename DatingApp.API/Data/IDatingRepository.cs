using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;

         Task<bool> SaveAll(); //check if there is single/multiple or no save to db
    
        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUser(int id);
    }

}