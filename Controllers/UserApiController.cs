using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Business.Validators;
using GoodJobGames.Data.EntityModels;
using GoodJobGames.Models.CustomExceptions;
using GoodJobGames.Models.Requests;
using GoodJobGames.Models.Responses;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;
using GoodJobGames.Models;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Models.CacheModel;
using GoodJobGames.Models.Requests.Import;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Renci.SshNet.Messages;
using Microsoft.EntityFrameworkCore.Internal;

namespace GoodJobGames.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<UserApiController> _logger;
        private readonly IUserService _userService;
        private readonly IScoreService _scoreService;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ICountryService _countryService;

        public UserApiController(ILogger<UserApiController> logger,
            IUserService userService,
            IScoreService scoreService,
            IValidatorResolver validatorResolver,
            IMapper mapper,
            ICacheService cacheService,
            ICountryService countryService
            )
        {
            _logger = logger;
            _userService = userService;
            _scoreService = scoreService;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
            _cacheService = cacheService;
            _countryService = countryService;
        }

        #region User

    /// <summary>
    /// Creates user
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
        [HttpPost("create")]
        public async Task<UserResponse> CreateUser([FromBody] UserRequest userRequest)
        {
            /* VALIDATE */
            var validator = _validatorResolver.Resolve<UserRequestValidator>();
            ValidationResult validationResult = validator.Validate(userRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString());
            }

            try
            {
                var user = _mapper.Map<User>(userRequest);
                var country = await _countryService.GetCountryByIsoCode(userRequest.CountryIsoCode);
                if (country == null)
                    throw new NotFoundException(nameof(Country));
                user.CountryId = country.Id;

                var userResponse = await _userService.CreateUser(user);
                if (userResponse != null)
                {
                    await generateScoreForNewUser(userResponse.GID);
                    var userResponseModel = _mapper.Map<UserResponse>(userResponse);

                    _cacheService.HashSet(new UserCacheModel
                    {
                        GID = userResponseModel.GID,
                        Username = userResponseModel.Username,
                        CountryIsoCode = userResponseModel.CountryIsoCode
                    });
                    userResponseModel.Rank = await addToCacheAndGetRank(userResponse.GID, country.CountryIsoCode); //Adds score info to sortedset
                    return userResponseModel;
                }
                throw new ConflictException("Create user failed");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpGet("profile/{userId}")]
        public async Task<UserResponse> GetUser([FromRoute] Guid userId)
        {
            if (userId == null)
                throw new NotFoundException("User can not be found");

            UserResponse userResponse = new UserResponse();
            if (await _cacheService.IsHashExist(userId))
            {
                var cachedUser = await _cacheService.HashGetAll(userId);
                userResponse.GID = userId;
                userResponse.Username = cachedUser.GetValueOrDefault("Username");
                userResponse.CountryIsoCode = cachedUser.GetValueOrDefault("CountryIsoCode");
            }
            else
            {
                var user = await _userService.GetUserByGuid(userId);
                userResponse = _mapper.Map<UserResponse>(user);

                _cacheService.HashSet(new UserCacheModel
                {
                    GID = userResponse.GID,
                    Username = userResponse.Username,
                    CountryIsoCode = userResponse.CountryIsoCode
                });
            }

            LeaderboardCacheModel cacheModel = new LeaderboardCacheModel { Id = userId };
            string key = $"{CacheKeyConstants.LEADERBOARD_KEY}.{userResponse.CountryIsoCode}";
            userResponse.Rank =  await addToCacheAndGetRank(userResponse.GID, userResponse.CountryIsoCode); //Adds score info to sortedset
            userResponse.Score = await _cacheService.SortedSetGetScore(key, cacheModel);

            return userResponse;
        }
        #endregion

        #region Bulk Import

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("BulkImportUser")]
        public async Task BulkImportUser([FromBody] BulkImportUserRequest request)
        {
            /* VALIDATE */
            var validator = _validatorResolver.Resolve<BulkImportUserRequestValidator>();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString());
            }

            try
            {
                Random rnd = new Random();
                var countryList = await _countryService.GetAllCountry();
                List<User> userList = new List<User>();
                for (int i = 1; i <= request.NumberOfUsers; i++)
                {
                    userList.Add(generateUserDTO(request.UsernamePrefix, request.FixedPassword, rnd.Next(1, countryList.Count), i));
                    if (i % 100 == 0)
                    {
                        await _userService.CreateUserRange(userList);
                        userList.Clear();
                    }
                }
                if (userList.Count > 0)
                    await _userService.CreateUserRange(userList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private User generateUserDTO(string namePrefix, string fixedPassword, int countryId, int counter)
        {
            return new User
            {
                GID = Guid.NewGuid(),
                CountryId = countryId,
                IsActive = true,
                IsDeleted = false,
                UpdatedOnUtc = DateTime.Now,
                CreatedOnUtc = DateTime.Now,
                Password = fixedPassword != null ? fixedPassword : "123456",
                Username = $"{namePrefix}-{counter}",
            };
        }

        private async Task generateScoreForNewUser(Guid guid)
        {
            //After Insert Trigger?
            await _scoreService.SubmitScore(new UserScore()
            {
                UserId = guid,
                Score = 0
            });
        }

        private async Task<int> addToCacheAndGetRank(Guid guid, string countryIsoCode)
        {
            //On Score Change
            LeaderboardCacheModel cacheModel = new LeaderboardCacheModel
            {
                Id = guid
            };
            string key = $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryIsoCode}";
            int rank = await _cacheService.SortedSetGetRank(key, cacheModel); 
            if (rank == -1)
                await _cacheService.SortedSetAdd(key, 0, cacheModel);
            else
                rank = await _cacheService.SortedSetGetRank(key, cacheModel);
            return rank;
        }
        #endregion
    }
}
