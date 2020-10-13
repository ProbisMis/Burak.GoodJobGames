using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GoodJobGames.Business.Validators;
using GoodJobGames.Data;
using GoodJobGames.Data.EntityModels;
using GoodJobGames.Models.Requests;
using GoodJobGames.Models.Responses;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodJobGames.Models.CustomExceptions;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public async Task<User> Authenticate(string username, string password)
        {
            var user = _dataContext.Users.Where(x => x.Username == username && x.Password == password).First();

            // Kullanici bulunamadıysa null döner.
            if (user == null)
                return null;

            // Authentication(Yetkilendirme) başarılı ise JWT token üretilir.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppConstants.JWTSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // Sifre null olarak gonderilir.
            user.Password = null;
            
            return user;
        }

        #region User CRUD
        public async Task<User> CreateUser(User userRequest)
        {
            var updateDate = DateTime.Now;

            userRequest.CreatedOnUtc = updateDate;
            userRequest.UpdatedOnUtc = updateDate;
            userRequest.IsDeleted = false;
            userRequest.GID = Guid.NewGuid();

            
            var user = _dataContext.Users.Add(userRequest);

            await _scoreService.SubmitScore(new Score()
            {
                UserId = userRequest.GID,
                UserScore = 0
            });

            await _dataContext.SaveChangesAsync();

            return user.Entity;
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
