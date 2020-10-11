using AutoMapper;
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
        private readonly IMapper _mapper;

        public LeaderboardApiController(ILogger<LeaderboardApiController> logger,
            IValidatorResolver validatorResolver,
            IMapper mapper
            )
        {
            _logger = logger;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
        }
    }
}
