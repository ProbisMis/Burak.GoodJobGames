using FluentValidation;
using FluentValidation.Results;

namespace GoodJobGames.Utilities.ValidationHelper
{
    public static class PreValidations
    {
        public static bool NotNullPreValidation<T>(ValidationContext<T> context, ValidationResult result)
        {
            var shouldContinue = true;

            if (context.InstanceToValidate == null)
            {
                shouldContinue = false;
                result.Errors.Add(new ValidationFailure(typeof(T).Name, "Object should not be null"));
            }

            return shouldContinue;
        }
    }
}