using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        public Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<User> next)
        {
            throw new NotImplementedException();
        }
    }
}
