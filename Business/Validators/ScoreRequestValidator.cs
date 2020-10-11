using Burak.GoodJobGames.Models.Requests;
using Burak.GoodJobGames.Utilities.ValidationHelper;
using FluentValidation;
using FluentValidation.Results;

namespace Burak.GoodJobGames.Business.Validators
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
