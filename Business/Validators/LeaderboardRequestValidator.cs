using FluentValidation;
using FluentValidation.Results;
using GoodJobGames.Models.Requests;
using GoodJobGames.Utilities.ValidationHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Validators
{
    public class LeaderboardRequestValidator : AbstractValidator<LeaderboardRequest>
    {
        protected override bool PreValidate(ValidationContext<LeaderboardRequest> context, ValidationResult result)
           => PreValidations.NotNullPreValidation(context, result);

        public LeaderboardRequestValidator()
        {
            RuleFor(r => r.CountryIsoCode).NotNull();
        }
    }
}
