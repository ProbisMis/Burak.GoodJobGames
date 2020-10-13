using GoodJobGames.Models.Requests;
using GoodJobGames.Utilities.ValidationHelper;
using FluentValidation;
using FluentValidation.Results;

namespace GoodJobGames.Business.Validators
{
    public class ScoreRequestValidator : AbstractValidator<ScoreRequest>
    {
        protected override bool PreValidate(ValidationContext<ScoreRequest> context, ValidationResult result)
           => PreValidations.NotNullPreValidation(context, result);

        public ScoreRequestValidator()
        {
            RuleFor(r => r.UserId).NotNull();
            RuleFor(r => r.UserScore).NotNull();
        }
    }
}
