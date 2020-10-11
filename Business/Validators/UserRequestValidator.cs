using Burak.GoodJobGames.Models.Requests;
using Burak.GoodJobGames.Utilities.ValidationHelper;
using FluentValidation;
using FluentValidation.Results;

namespace Burak.GoodJobGames.Business.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        protected override bool PreValidate(ValidationContext<UserRequest> context, ValidationResult result)
           => PreValidations.NotNullPreValidation(context, result);

        public UserRequestValidator()
        {
            RuleFor(r => r.Username).NotNull();
        }
    }
}
