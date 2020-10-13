using AutoMapper;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace GoodJobGames.Controllers
{

    [ApiController]
    [Route("score")]
    public class LeaderboardApiController : ControllerBase
    {
        private readonly ILogger<LeaderboardApiController> _logger;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IScoreService _scoreService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public LeaderboardApiController(ILogger<LeaderboardApiController> logger,
            IValidatorResolver validatorResolver,
            IMapper mapper,
            IScoreService scoreService,
            IUserService userService,
            IDistributedCache distributedCache
            )
        {
            _logger = logger;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
            _scoreService = scoreService;
            _userService = userService;
            _distributedCache = distributedCache;
        }


        /// <summary>
        /// Gets  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task GetLeaderboard()
        {
            //<List<LeaderboardResponse>>
            await RedisHelper.SetAsync("hello", "world");
            await _distributedCache.SetAsync("hello", Encoding.ASCII.GetBytes("world"));
        }

        /// <summary>
        /// Gets  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpGet("try")]
        public async Task<string> GetLeaderboardTry()
        {
            //<List<LeaderboardResponse>>
            //var x = await RedisHelper.GetAsync("hello");
            var x =  await _distributedCache.GetAsync("hello");
            return Encoding.ASCII.GetString(x);
        }
    }
}
