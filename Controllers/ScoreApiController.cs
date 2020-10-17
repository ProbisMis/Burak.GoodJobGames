using AutoMapper;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Business.Validators;
using GoodJobGames.Data.EntityModels;
using GoodJobGames.Models.Requests;
using GoodJobGames.Models.Responses;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Models.CacheModel;

namespace GoodJobGames.Controllers
{
    [ApiController]
    [Route("score")]
    public class ScoreApiController : ControllerBase
    {
        private readonly ILogger<ScoreApiController> _logger;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IScoreService _scoreService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public ScoreApiController(ILogger<ScoreApiController> logger,
            IValidatorResolver validatorResolver,
            IMapper mapper,
            IScoreService scoreService,
            ICacheService cacheService,
            IUserService userService
            )
        {
            _logger = logger;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
            _scoreService = scoreService;
            _userService = userService;
            _cacheService = cacheService;
        }

        [HttpPost("submit")]
        public async Task<ScoreResponse> SubmitScore([FromBody] ScoreRequest scoreRequest)
        {
            /* VALIDATE */
            var validator = _validatorResolver.Resolve<ScoreRequestValidator>();
            ValidationResult validationResult = validator.Validate(scoreRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString());
            }

            ScoreResponse scoreResponse = new ScoreResponse();
            var user = await _userService.GetUserByGuid(scoreRequest.UserId);
            var userResponseModel = _mapper.Map<UserResponse>(user);
            

            var score = _mapper.Map<UserScore>(scoreRequest);
            await _scoreService.SubmitScore(score);

            scoreResponse.UserId = user.GID;
            scoreResponse.Timestamp = DateTime.Now;
            scoreResponse.Score = user.Score.Score;
            LeaderboardCacheModel cacheModel = new LeaderboardCacheModel{
                Id = scoreRequest.UserId
            };
            string key = $"{CacheKeyConstants.LEADERBOARD_KEY}.{user.Country.CountryIsoCode}";
            var rank = await  _cacheService.SortedSetGetRank(key, cacheModel);
            if (rank == -1)
            {
                //Add to cache
                await _cacheService.SortedSetAdd(key, userResponseModel.Score, cacheModel);
            }
            else
            {
                //Increment cache
                await _cacheService.SortedSetIncrement(key, userResponseModel.Score, cacheModel);
            }

            return scoreResponse;
        }
    }
}
