using FluentValidation;
using FluentValidation.Results;
using GoodJobGames.Models.Requests.Import;
using GoodJobGames.Utilities.ValidationHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Validators
{
    public class BulkImportUserRequestValidator : AbstractValidator<BulkImportUserRequest>
    {
        protected override bool PreValidate(ValidationContext<BulkImportUserRequest> context, ValidationResult result)
           => PreValidations.NotNullPreValidation(context, result);

        public BulkImportUserRequestValidator()
        {
            RuleFor(r => r.NumberOfUsers).NotNull();
        }
    }
}
