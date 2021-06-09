using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TandemChallenge.Domain
{
    /// <summary>
    /// Handles requests to create a new user.
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateUserCommandHandler> logger;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<CreateUserCommandHandler> logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Checking to see if the email '{request.EmailAddress}' exists");

            if(await EmailExists(request.EmailAddress))
            {
                throw new ValidationException($"The email '{request.EmailAddress}' has already been registered.");
            }

            logger.LogDebug($"Mapping the request with the email '{request.EmailAddress}' to a user");
            var user = mapper.Map<User>(request);

            logger.LogDebug($"Saving the user with the email '{request.EmailAddress}' to the repository");
            await userRepository.AddAsync(user);

            return user;
        }

        private async Task<bool> EmailExists(string emailAddress)
        {
            var searchFilter = new UserSearchCriteria(emailAddress);
            var matchingUsers = await userRepository.SearchAsync(searchFilter);

            return matchingUsers.Any();
        }
    }
}
