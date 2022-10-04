using ESA.Core.Models.Account;
using FluentValidation;

namespace ESA.Core.Validators
{
    public class AccountValidator : AbstractValidator<AccountBaseInfo>
    {
        public AccountValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Is required.");
            RuleFor(x => x.Picture).NotNull().NotEmpty().WithMessage("Is required.");
        }
    }

    public class AccountProfileValidator : AbstractValidator<AccountProfileBase>
    {
        public AccountProfileValidator()
        {
            RuleFor(x => x.AccountId).NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.DateOfBirth).NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.NationalityCode)
                .Must(x => x.Length >= 2 && x.Length <= 3)
                .NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.AboutMe)
                .MaximumLength(200)
                .NotNull().NotEmpty().WithMessage("Is required.");
        }
    }

    public class AccountSurveyValidator : AbstractValidator<AccountSurveyBaseInfo>
    {
        public AccountSurveyValidator()
        {
            RuleFor(x => x.AccountId).NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.MediaInfo)
                .MaximumLength(40)
                .NotNull().NotEmpty().WithMessage("Is required.");
        }
    }
}
