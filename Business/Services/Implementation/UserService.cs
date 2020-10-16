using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GoodJobGames.Data;
using GoodJobGames.Data.EntityModels;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using GoodJobGames.Business.Services.Interface;

namespace GoodJobGames.Business.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IScoreService _scoreService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(DataContext dataContext, IMapper mapper, IScoreService scoreService,
             IValidatorResolver validatorResolver, IConfiguration configuration, ILogger<UserService> logger)
        {
            _dataContext = dataContext;
            _validatorResolver = validatorResolver;
            _configuration = configuration;
            _logger = logger;
            _scoreService = scoreService;
        }

        #region User CRUD
        public async Task<User> CreateUser(User user)
        {
            var updateDate = DateTime.Now;
            user.CreatedOnUtc = updateDate;
            user.UpdatedOnUtc = updateDate;
            user.IsDeleted = false;
            user.IsActive = true;
            user.GID = Guid.NewGuid();
            
            var newUser =  _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return newUser.Entity;
        }

        public async Task<User> GetUserById(int userId)
        {
            var user = _dataContext.Users.Include(x => x.Score).Where(x => x.Id == userId && !x.IsDeleted && x.IsActive).First();
            return user;
        }

        public async Task<User> GetUserByGuid(Guid userGuid)
        {
            var user = _dataContext.Users.Include(x => x.Score).Where(x => x.GID == userGuid && !x.IsDeleted && x.IsActive).First();
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = _dataContext.Users.Include(x => x.Score).Where(x => x.Username == username && !x.IsDeleted && x.IsActive).First();
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
