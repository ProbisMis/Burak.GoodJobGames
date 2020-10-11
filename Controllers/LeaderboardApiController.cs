using AutoMapper;
using Burak.GoodJobGames.Business.Services.Interface;
using Burak.GoodJobGames.Models.CustomExceptions;
using Burak.GoodJobGames.Models.Responses;
using Burak.GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Controllers
{
    public class LeaderboardApiController : ControllerBase
    {
        private readonly ILogger<LeaderboardApiController> _logger;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IScoreService _scoreService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LeaderboardApiController(ILogger<LeaderboardApiController> logger,
            IValidatorResolver validatorResolver,
            IMapper mapper,
            IScoreService scoreService,
            IUserService userService
            )
        {
            _logger = logger;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
            _scoreService = scoreService;
            _userService = userService;
        }


        ///// <summary>
        ///// Gets  user
        ///// </summary>
        ///// <param name="userRequest"></param>
        ///// <returns></returns>
        //[HttpGet("")]
        //public async Task<List<LeaderboardResponse>> GetLeaderboard()
        //{
        //    var result = _scoreService.

        //    return userResponseModel;
        //}
    }
}
