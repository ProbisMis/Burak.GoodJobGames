using AutoMapper;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data.EntityModels;
using GoodJobGames.Models;
using GoodJobGames.Models.Requests;
using GoodJobGames.Models.Responses;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodJobGames.Controllers
{

    [ApiController]
    [Route("leaderboard")]
    public class LeaderboardApiController : ControllerBase
    {
        private readonly ILogger<LeaderboardApiController> _logger;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IScoreService _scoreService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ICountryService _countryService;

        public LeaderboardApiController(ILogger<LeaderboardApiController> logger,
            IValidatorResolver validatorResolver,
            IMapper mapper,
            IScoreService scoreService,
            IUserService userService,
            ICountryService countryService,
            ICacheService cacheService
            )
        {
            _logger = logger;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
            _scoreService = scoreService;
            _userService = userService;
            _cacheService = cacheService;
            _countryService = countryService;
        }

        /// <summary>
        /// Gets  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<string> GetAll()
        {
            var countryList = await _countryService.GetAllCountry();
            List<UserResponse> result;
            if (_cacheService.hasAny(CacheKeyConstants.ALL_LEADERBOARD_KEY))
            {
                result =  _cacheService.SortedSetGetAll<UserResponse>(CacheKeyConstants.ALL_LEADERBOARD_KEY);
            }
            else
            {
                int j = 0;
                for (int i = 1; i < countryList.Count; i++)
                {   
                    if (i ==1) 
                        _cacheService.SortedSetIntersect($"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i-1].CountryIsoCode}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY);
                    else if (i == countryList.Count - 1)
                        _cacheService.SortedSetIntersect($"{CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_KEY);
                    else
                        _cacheService.SortedSetIntersect($"{CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY}.{countryList[i - 1].CountryIsoCode}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY);
                }
            }
            var x = _cacheService.SortedSetGetAll<string>(CacheKeyConstants.LEADERBOARD_KEY);
            return JsonConvert.SerializeObject(x);
        }

        /// <summary>
        /// Gets  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpGet("flush")]
        public async Task FlushDB()
        {
            _cacheService.Clear();
        }

        /// <summary>
        /// Gets  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("flushbykey")]
        public async Task FlushDB([FromBody] UserRequest user)
        {
            _cacheService.Remove(user.Username);

        }

    }
}
