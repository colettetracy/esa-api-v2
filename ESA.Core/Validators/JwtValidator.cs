using ESA.Core.Models.Session;
using FluentValidation;

namespace ESA.Core.Validators
{
    public class JwtValidator : AbstractValidator<JwtInfo>
    {
        public JwtValidator()
        {
            RuleFor(x => x.Identifier)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Ingrese un Iientificador válido");

            RuleFor(x => x.ServiceName)
                .NotEmpty()
                .WithMessage("Ingrese el nombre del servicio");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Ingrese el nombre de usuario");
        }
    }
}
