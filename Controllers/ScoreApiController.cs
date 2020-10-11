using AutoMapper;
using Burak.GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Controllers
{
    [ApiController]
    [Route("score")]
    public class ScoreApiController : ControllerBase
    {
        private readonly ILogger<ScoreApiController> _logger;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IMapper _mapper;

        public ScoreApiController(ILogger<ScoreApiController> logger,
            IValidatorResolver validatorResolver,
            IMapper mapper
            )
        {
            _logger = logger;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
        }

        //[HttpPost("submit")]
        //public async Task<ScoreResponse> SubmitScore([FromBody] ScoreRequest scoreRequest)
        //{
        //    /* VALIDATE */
        //    var validator = _validatorResolver.Resolve<ScoreRequestValidator>();
        //    ValidationResult validationResult = validator.Validate(scoreRequest);
        //    if (!validationResult.IsValid)
        //    {
        //        throw new ValidationException(validationResult.ToString());
        //    }

        //    var user = _mapper.Map<Score>(scoreRequest);

        //    var userResponse = _userService.CreateUser(user);

        //    var userResponseModel = _mapper.Map<UserResponse>(userResponse.Result);

        //    return userResponseModel;
        //}
    }
}
