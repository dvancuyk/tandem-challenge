using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TandemChallenge.Domain.Validation;

namespace TandemChallenge.Domain
{
    /// <summary>
    /// Valiidates the <see cref="CreateUserCommand"/> prior to the <see cref="CreateUserCommandHandler"/> processing the request.
    /// </summary>
    /// <devdoc>
    /// I made the decision to move this here b/c I added some validation logic that was domain specific and wanted to keep everything centralized.
    /// Using the preprocessing pipeline behavior means that this can be added or removed with modifying the handling logic.
    /// </devdoc>
    public class ValidateCreateUserCommand : IPipelineBehavior<CreateUserCommand, User>
    {
        private readonly IUserRepository userRepository;

        public ValidateCreateUserCommand(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<User> next)
        {
            var validator = new CreateUserCommandValidator();

            var results = validator.Validate(request);
            var emailProperty = nameof(request.EmailAddress);
            if (!results.Errors.Any(e => e.PropertyName == emailProperty) && await EmailExists(request.EmailAddress))
            {
                results.Errors.Add(new FluentValidation.Results.ValidationFailure(emailProperty, $"The email '{request.EmailAddress}' has already been registered."));
            }

            if(!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            return await next();
        }

        private async Task<bool> EmailExists(string emailAddress)
        {
            var searchFilter = new UserSearchCriteria(emailAddress);
            var matchingUsers = await userRepository.SearchAsync(searchFilter);

            return matchingUsers.Any();
        }
    }
}
