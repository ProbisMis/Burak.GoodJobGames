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
                if (country != null)
                    user.CountryId = country.Id;

                var userResponse = await _userService.CreateUser(user);
                if (userResponse != null)
                {
                    //After Insert
                    await _scoreService.SubmitScore(new Score()
                    {
                        UserId = userResponse.GID,
                        UserScore = 0
                    });

                    var userResponseModel = _mapper.Map<UserResponse>(userResponse);
                    userResponseModel.Score = 0;
                    
                    //On Score Change
                    string key = $"{CacheKeyConstants.LEADERBOARD_KEY}.{country.CountryIsoCode}";
                    _cacheService.SortedSetAdd(key, userResponseModel.Score, userResponseModel);
                    var rank = _cacheService.SortedSetGetRank(key, userResponseModel);
                    userResponseModel.Rank = rank;
                    return userResponseModel;
                }
                return null; //TODO: Good message
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

            var user = await _userService.GetUserByGuid(userId);
            var userResponseModel = _mapper.Map<UserResponse>(user);

            string key = $"{CacheKeyConstants.LEADERBOARD_KEY}.{user.Country.CountryIsoCode}";
            userResponseModel.Rank = _cacheService.SortedSetGetRank(key, userResponseModel);

            return userResponseModel;
        }
        #endregion
    } 
}
