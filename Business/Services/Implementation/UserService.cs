using AutoMapper;
using GoodJobGames.Data;
using GoodJobGames.Data.EntityModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodJobGames.Business.Services.Interface;
using System.Collections.Generic;

namespace GoodJobGames.Business.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IScoreService _scoreService;

        public UserService(DataContext dataContext, IMapper mapper, IScoreService scoreService
              )
        {
            _dataContext = dataContext;
            _scoreService = scoreService;
        }

        #region User CRUD
        public async Task<User> CreateUser(User user)
        {
            user.GID = Guid.NewGuid();
            
            var newUser =  _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return newUser.Entity;
        }

        public async Task CreateUserRange(List<User> users)
        {
                foreach (var user in users)
                {
                    _dataContext.Entry(user).State = EntityState.Added;
                    _dataContext.Entry(user.CountryId).State = EntityState.Unchanged;
                }
                _dataContext.Users.AddRange(users);
                _dataContext.SaveChanges();
        }

        public async Task<User> GetUserById(int userId)
        {
            var user = _dataContext.Users.Include(x => x.Score).Include(x => x.Country).Where(x => x.Id == userId && !x.IsDeleted && x.IsActive).FirstOrDefault();
            return user;
        }

        public async Task<User> GetUserByGuid(Guid userGuid)
        {
            var user = _dataContext.Users.Include(x => x.Score).Include(x=>x.Country).Where(x => x.GID == userGuid && !x.IsDeleted && x.IsActive).FirstOrDefault();
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = _dataContext.Users.Include(x => x.Score).Include(x => x.Country).Where(x => x.Username == username && !x.IsDeleted && x.IsActive).FirstOrDefault();
            return user;
        }

        public async Task<User> UpdateUser(User userRequest)
        {
            var updateDate = DateTime.Now;

            userRequest.UpdatedOnUtc = updateDate;

            var user = _dataContext.Users.Update(userRequest);
            await _dataContext.SaveChangesAsync();

            return user.Entity;
        }

        #endregion
    }
}
