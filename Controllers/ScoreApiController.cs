using AutoMapper;
using Burak.GoodJobGames.Business.Services.Interface;
using Burak.GoodJobGames.Business.Validators;
using Burak.GoodJobGames.Data.EntityModels;
using Burak.GoodJobGames.Models.Requests;
using Burak.GoodJobGames.Models.Responses;
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
        private readonly IScoreService _scoreService;
        private readonly IMapper _mapper;

        public ScoreApiController(ILogger<ScoreApiController> logger,
            IValidatorResolver validatorResolver,
            IMapper mapper,
            IScoreService scoreService
            )
        {
            _logger = logger;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
            _scoreService = scoreService;
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

            var score = _mapper.Map<Score>(scoreRequest);

            await _scoreService.SubmitScore(score);

            var response = _scoreService.GetScoreByUserId(scoreRequest.UserId);
            return _mapper.Map<ScoreResponse>(response);
        }
    }
}
