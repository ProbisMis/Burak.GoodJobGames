using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Burak.GoodJobGames.Business.Services.Interface;
using Burak.GoodJobGames.Business.Validators;
using Burak.GoodJobGames.Data.EntityModels;
using Burak.GoodJobGames.Models.CustomExceptions;
using Burak.GoodJobGames.Models.Requests;
using Burak.GoodJobGames.Models.Responses;
using Burak.GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Burak.GoodJobGames.Controllers
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

        public UserApiController(ILogger<UserApiController> logger,
            IUserService userService,
            IScoreService scoreService,
            IValidatorResolver validatorResolver,
            IMapper mapper
            )
        {
            _logger = logger;
            _userService = userService;
            _scoreService = scoreService;
            _validatorResolver = validatorResolver;
            _mapper = mapper;
        }

        #region Authorization

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<UserResponse> Authenticate([FromBody]LoginRequest userRequest)
        {
                var user = await _userService.Authenticate(userRequest.Username, userRequest.Password);

                return _mapper.Map<UserResponse>(user);
        }

        #endregion

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

            var user = _mapper.Map<User>(userRequest);

            var userResponse = _userService.CreateUser(user);

            var userResponseModel = _mapper.Map<UserResponse>(userResponse.Result);
            userResponseModel.Score = 0;
            //TODO: GET RANK


            return userResponseModel;
        }

        /// <summary>
        /// Updates  user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPut("")]
        public async Task<UserResponse> UpdateUser([FromBody] UserRequest userRequest)
        {
            /* VALIDATE */
            var validator = _validatorResolver.Resolve<UserRequestValidator>();
            ValidationResult validationResult = validator.Validate(userRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString());
            }

            var user = _mapper.Map<User>(userRequest);

            var userResponse = _userService.UpdateUser(user);

            var userResponseModel = _mapper.Map<UserResponse>(userResponse.Result);

            return userResponseModel;
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

            var user = _userService.GetUserByGuid(userId);

            var userResponseModel = _mapper.Map<UserResponse>(user.Result);

            return userResponseModel;
        }
        #endregion
    } 
}
