using FluentValidation;

namespace TandemChallenge.Domain.Validation
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First Name is required");
            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last name is required");
            RuleFor(m => m.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address must be provided");
            RuleFor(m => m.PhoneNumber)
                .Matches(@"^[2-9]\d{2}-\d{3}-\d{4}$")
                .WithMessage("Phone number must be a valid phone number");
        }
    }
}

