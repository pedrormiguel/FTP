using CORE.Domain.Entities;
using FluentValidation;

namespace CORE.Domain.Validation
{
    public class CredentialValidator : AbstractValidator<Credential>
    {
        public CredentialValidator()
        {
            RuleFor(x => x.HostName)
                .NotEmpty()
                .NotNull().WithMessage(x => $"{nameof(x.HostName)} must not be null.");
            
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();
            
            RuleFor(x => x.UserName).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();

            RuleFor(x => x.Port)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
