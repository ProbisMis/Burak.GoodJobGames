using AutoMapper;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data.EntityModels;
using GoodJobGames.Models.CacheModel;
using GoodJobGames.Models.CustomExceptions;
using GoodJobGames.Models.Requests;
using GoodJobGames.Models.Responses;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        ///// <summary>
        ///// Gets  user
        ///// </summary>
        ///// <param name="userRequest"></param>
        ///// <returns></returns>
        //[HttpGet("")]
        //public async Task<string> GetAll()
        //{
        //    try
        //    {
        //        var countryList = await _countryService.GetAllCountry();
        //        List<LeaderboardCacheModel> result;
        //        if (_cacheService.hasAny(CacheKeyConstants.ALL_LEADERBOARD_KEY))
        //        {
        //            result = _cacheService.SortedSetGetAll(CacheKeyConstants.ALL_LEADERBOARD_KEY);
        //        }
        //        else
        //        {
        //            for (int i = 1; i < countryList.Count; i++)
        //            {
        //                if (i == 1)
        //                    await _cacheService.SortedSetIntersect($"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i - 1].CountryIsoCode}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY);
        //                else if (i == countryList.Count - 1)
        //                    await _cacheService.SortedSetIntersect($"{CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_KEY);
        //                else
        //                    await _cacheService.SortedSetIntersect($"{CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY}", $"{CacheKeyConstants.LEADERBOARD_KEY}.{countryList[i].CountryIsoCode}", CacheKeyConstants.ALL_LEADERBOARD_TEMP_KEY);
        //            }
        //        }
        //        result = _cacheService.SortedSetGetAll(CacheKeyConstants.ALL_LEADERBOARD_KEY);
        //        return JsonConvert.SerializeObject(result);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
           
        //}

        
        /// <summary>
        /// Returns global leaderboard by page each page has hundred record
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("{pageNumber}")]
        public async Task<LeaderboardListResponse> GetAll([FromRoute] int pageNumber)
        {
            try
            {
                var countryList = await _countryService.GetAllCountry();
                LeaderboardListResponse response = new LeaderboardListResponse();
                response.LeaderboardResponses = new List<LeaderboardResponse>();
                LeaderboardResponse leaderboardResponse;
                if (_cacheService.hasAny(CacheKeyConstants.ALL_LEADERBOARD_KEY))
                {
                    List<LeaderboardCacheModel>  result = _cacheService.SortedSetGetAll(CacheKeyConstants.ALL_LEADERBOARD_KEY, pageNumber);

                    foreach (var item in result)
                    {
                        leaderboardResponse = await GenerateLeaderboardResponse(item.Id, CacheKeyConstants.ALL_LEADERBOARD_KEY);
                        response.LeaderboardResponses.Add(leaderboardResponse);
                    }
                }
                else
                {
                    int rankCounter = 0;
                    var scores = await _scoreService.GetScores(100);
                    if (scores == null)
                        throw new NotFoundException(nameof(UserScore));
                    foreach (var item in scores)
                    {
                        rankCounter++;
                        leaderboardResponse = await GenerateLeaderboardResponse(item.UserId, CacheKeyConstants.ALL_LEADERBOARD_KEY, rankCounter);

                        await _cacheService.SortedSetAdd(CacheKeyConstants.ALL_LEADERBOARD_KEY, item.Score, new LeaderboardCacheModel { Id = Guid.Parse(item.UserId.ToString()) });
                        response.LeaderboardResponses.Add(leaderboardResponse);
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Gets country specific leaderboard
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<LeaderboardListResponse> GetAllByKey([FromBody] LeaderboardRequest leaderboardRequest)
        {
            LeaderboardListResponse response = new LeaderboardListResponse();
            response.LeaderboardResponses = new List<LeaderboardResponse>();
            LeaderboardResponse leaderboardResponse;

            string key = $"{CacheKeyConstants.LEADERBOARD_KEY}.{leaderboardRequest.CountryIsoCode}";
            try
            {
                if (_cacheService.hasAny(key))
                {
                    var result = _cacheService.SortedSetGetAll(key, leaderboardRequest.PageNumber);
                    
                    foreach (var item in result)
                    {
                        leaderboardResponse = await GenerateLeaderboardResponse(item.Id, key);
                        response.LeaderboardResponses.Add(leaderboardResponse);
                    }
                }
                else
                {
                    int rankCounter = 0;
                    var country = await _countryService.GetCountryByIsoCode(leaderboardRequest.CountryIsoCode);
                    var scores = await _scoreService.GetScoresByCountry(leaderboardRequest.PageNumber, country.Id);
                    if (scores == null)
                        throw new NotFoundException(nameof(UserScore));
                    foreach (var item in scores)
                    {
                        rankCounter++;
                        leaderboardResponse = await GenerateLeaderboardResponse(item.UserId, key, rankCounter);

                        await _cacheService.SortedSetAdd(CacheKeyConstants.ALL_LEADERBOARD_KEY, item.Score, new LeaderboardCacheModel { Id = item.UserId});
                        response.LeaderboardResponses.Add(leaderboardResponse);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }

        /// <summary>
        /// Flush all databases & keys
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpGet("flush")]
        public async Task FlushDB()
        {
            _cacheService.Clear();
        }

        /// <summary>
        /// Flush specific key
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("flushbykey")]
        public async Task FlushDB([FromBody] CacheKeyModel request)
        {
            _cacheService.Remove(request.Key);

        }

        private async Task<LeaderboardResponse> GenerateLeaderboardResponse(Guid guid, string key, int rank = -1)
        {
            LeaderboardResponse leaderboardResponse;
            var leaderboardCacheModel = new LeaderboardCacheModel { Id = Guid.Parse(guid.ToString()) };
            var userCachemodel = new UserCacheModel { GID = Guid.Parse(guid.ToString()) };
            if (await _cacheService.IsHashExist(guid))
            {
                leaderboardResponse = new LeaderboardResponse
                {
                    Username = await _cacheService.HashGet(userCachemodel, "Username"),
                    CountryName = await _cacheService.HashGet(userCachemodel, "CountryIsoCode"),
                    Rank = rank == -1  ? await addToCacheAndGetRank(key, leaderboardCacheModel) : rank,
                    Score = await _cacheService.SortedSetGetScore(key, leaderboardCacheModel)
                };
            }
            else
            {
                var user = await _userService.GetUserByGuid(guid);
                if (user == null) throw new NotFoundException(nameof(User));

                leaderboardResponse = new LeaderboardResponse
                {
                    Username = user.Username,
                    CountryName = user.Country.CountryIsoCode,
                    Rank = await addToCacheAndGetRank(key, leaderboardCacheModel),
                    Score = await _cacheService.SortedSetGetScore(key, leaderboardCacheModel)
                };
            }
            return leaderboardResponse;
        }

        private async Task<int> addToCacheAndGetRank(string key, LeaderboardCacheModel model)
        {
            int rank = -1;
            if (!await _cacheService.hasAny(key,model))
                await _cacheService.SortedSetAdd(key, 0, model);
            rank = await _cacheService.SortedSetGetRank(key, model);
            return rank;
        }

    }
}
