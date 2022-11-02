using ESA.Core.Models.Payment;
using FluentValidation;

namespace ESA.Core.Validators
{
    public class CouponValidators : AbstractValidator<CouponBaseInfo>
    {
        public CouponValidators()
        {
            RuleFor(x => x.Coupon)
                .NotNull().NotEmpty().WithMessage("Is required.")
                .MaximumLength(20);
        }

    }
}
