using FluentValidation;
using TandemChallenge.Api.Models;

namespace TandemChallenge.Api.Validation
{
    internal class AddUserViewModelValidator : AbstractValidator<AddUserViewModel>
    {
        public AddUserViewModelValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("First Name is required");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(m => m.EmailAddress).EmailAddress().WithMessage("A vliad email address must be provided");
            RuleFor(m => m.PhoneNumber).Matches(@"^[2-9]\d{2}-\d{3}-\d{4}$").WithMessage("Phone number must be a valid phone number");
        }
    }
}

