using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Burak.GoodJobGames.Data.EntityModels;

namespace Burak.GoodJobGames.Business.Services.Interface
{
    public interface IUserService
    {
        //Authentication
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();

        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<User> GetUserById(Guid userId);
        Task<User> DeleteUser(User user);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
    }
}
