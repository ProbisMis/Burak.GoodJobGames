using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodJobGames.Data.EntityModels;

namespace GoodJobGames.Business.Services.Interface
{
    public interface IUserService
    {
        Task<User> CreateUser(User user);
        Task CreateUserRange(List<User> user);
        Task<User> UpdateUser(User user);
        Task<User> GetUserById(int userId);
        Task<User> GetUserByGuid(Guid userGuid);
        Task<User> GetUserByUsername(string username);
    }
}
