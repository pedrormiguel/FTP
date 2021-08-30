using CORE.Domain.Entities;
using FluentValidation;

namespace CORE.Domain.Validation
{
    public class CredentialValidator : AbstractValidator<Credential>
    {
        public CredentialValidator()
        {
            RuleFor(x => x.HostName).NotEmpty()
                                    .NotNull()
                                    .WithMessage("HostName cannot be Empty");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName cannot be Empty");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be Empty");

            RuleFor(x => x.Port).NotEmpty()
                                .GreaterThan(0)
                                .WithMessage("Port cannot be Empty");
        }
    }
}
