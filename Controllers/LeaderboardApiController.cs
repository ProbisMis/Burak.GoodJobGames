using AutoMapper;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data.EntityModels;
using GoodJobGames.Models;
using GoodJobGames.Models.CacheModel;
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
            List<LeaderboardCacheModel> result;
            if (_cacheService.hasAny(CacheKeyConstants.ALL_LEADERBOARD_KEY))
            {
                result =  _cacheService.SortedSetGetAll(CacheKeyConstants.ALL_LEADERBOARD_KEY);
            }
            else
            {
                for (int i = 1; i < countryList.Count; i++)
                {   
                    if (i ==1)
                        await _cacheService.SortedSetIntersect($"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i-1].CountryIsoCode}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY);
                    else if (i == countryList.Count - 1)
                        await _cacheService.SortedSetIntersect($"{CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_KEY);
                    else
                        await _cacheService.SortedSetIntersect($"{CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY);
                }
            }
            result = _cacheService.SortedSetGetAll(CacheKeyConstants.LEADERBOARD_KEY);
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Gets  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<LeaderboardListResponse> GetAllByKey([FromBody] LeaderboardRequest leaderboardRequest)
        {
            LeaderboardListResponse response = new LeaderboardListResponse();
            string key = $"{CacheKeyConstants.LEADERBOARD_KEY}.{leaderboardRequest.CountryIsoCode}";
            if (_cacheService.hasAny(key))
            {
                var result = _cacheService.SortedSetGetAll($"{CacheKeyConstants.LEADERBOARD_KEY}.{leaderboardRequest.CountryIsoCode}");
                foreach (var item in result)
                {
                    var properties = await _cacheService.HashGetAll(Guid.Parse(item.Id.ToString()));
                    var leaderboardCacheModel = new LeaderboardCacheModel { Id = Guid.Parse(item.Id.ToString()) };
                    var leaderboardResponse = new LeaderboardResponse
                    {
                        Username = properties.GetValueOrDefault("Username"),
                        CountryName = properties.GetValueOrDefault("CountryIsoCode"),
                        Rank = await _cacheService.SortedSetGetRank(key,leaderboardCacheModel),
                        Score = await _cacheService.SortedSetGetScore(key, leaderboardCacheModel)
                    };
                    response.LeaderboardResponses.Add(leaderboardResponse);
                }
            }

            return response;
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
        public async Task FlushDB([FromBody] LeaderboardRequest leaderboardRequest)
        {
            _cacheService.Remove($"{CacheKeyConstants.LEADERBOARD_KEY}.{leaderboardRequest.CountryIsoCode}");

        }

    }
}
