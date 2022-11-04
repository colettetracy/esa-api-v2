using ESA.Core.Models.Course;
using ESA.Core.Models.Student;
using ESA.Core.Specs.Filters;
using FluentValidation;

namespace ESA.Core.Validators
{
    public class CourseValidator : AbstractValidator<CourseBaseInfo>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Code)
                .NotNull().NotEmpty().WithMessage("Is required.")
                .MaximumLength(20);

            RuleFor(x => x.Title)
                .NotNull().NotEmpty().WithMessage("Is required.")
                .MaximumLength(80);

            RuleFor(x => x.Subtitle)
                .NotNull().NotEmpty().WithMessage("Is required.")
                .MaximumLength(120);

            RuleFor(x => x.About).NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.Content).NotNull().NotEmpty().WithMessage("Is required.");

            RuleFor(x => x.CurrencyCode)
                .NotNull().NotEmpty().WithMessage("Is required.")
                .MaximumLength(10);

            //RuleFor(x => x.Price).GreaterThanOrEqualTo(0.0);
            RuleFor(x => x.Icon).NotNull().NotEmpty().WithMessage("Is required.");
        }
    }

    public class CourseFilterValidator : AbstractValidator<CourseFilter>
    {
        public CourseFilterValidator()
        {
            //RuleFor(x => x.Code).NotNull().NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Title)).WithMessage("Is required.");
            //RuleFor(x => x.Title).NotNull().NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Code)).WithMessage("Is required.");
        }
    }

    public class CalendarValidator : AbstractValidator<CalendarBaseInfo>
    {
        public CalendarValidator()
        {
            RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.TeacherId).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.FinishDate).WithMessage("Is required.");
            RuleFor(x => x.FinishDate).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Is required.");
        }
    }

    public class ScheduleValidator : AbstractValidator<ScheduleBaseInfo>
    {
        public ScheduleValidator()
        {
            RuleFor(x => x.CalendarId).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.Minutes).GreaterThan(0).WithMessage("Is required.");
        }
    }

    public class ReviewValidator : AbstractValidator<ReviewBaseInfo>
    {
        public ReviewValidator()
        {
            RuleFor(x => x.CalendarId).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.CourseStudentId).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.Comment).NotNull().NotEmpty().WithMessage("Is required.");
            RuleFor(x => x.Rating)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(5);
        }
    }

    public class StudentValidator : AbstractValidator<StudentBaseInfo>
    {
        public StudentValidator()
        {
            RuleFor(x => x.ScheduleId).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("Is required.");
        }
    }
    public class StudentDeleteValidator : AbstractValidator<StudentDeleteBaseInfo>
    {
        public StudentDeleteValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Is required.");
        }
    }
    public class StudentCouponValidator : AbstractValidator<StudentCouponBaseInfo>
    {
        public StudentCouponValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.Coupon).NotNull().NotEmpty().WithMessage("Is required.");
        }
    }
    public class StudentGroupValidator : AbstractValidator<StudentGroupCreate>
    {
        public StudentGroupValidator()
        {
            RuleFor(x => x.CalendarId).GreaterThan(0).WithMessage("Is required.");
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("Is required.");
        }
    }
}
