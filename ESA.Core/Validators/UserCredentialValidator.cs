using ESA.Core.Models.Auth;
using FluentValidation;

namespace ESA.Core.Validators
{
    public class UserCredentialValidator : AbstractValidator<UserCredential>
    {
        public UserCredentialValidator()
        {
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Empty username");

            RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Empty password");
        }
    }
}
