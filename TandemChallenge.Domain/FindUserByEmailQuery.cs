using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TandemChallenge.Domain
{
    /// <summary>
    /// Encapsulates a request to find a <see cref="User"/> which matches a given email.
    /// </summary>
    public class FindUserByEmailQuery : IRequest<User>
    {
        public FindUserByEmailQuery(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; private set; }
    }

    /// <summary>
    /// Handles requests to find a single user with the provided email address.
    /// </summary>
    public class FindUserByEmailHandler : IRequestHandler<FindUserByEmailQuery, User>
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<FindUserByEmailHandler> logger;

        public FindUserByEmailHandler(IUserRepository userRepository, ILogger<FindUserByEmailHandler> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<User> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Searching for users with the email address '{request.EmailAddress}'");
            var searchFilter = new UserSearchCriteria(request.EmailAddress);
            var matchingUsers = await userRepository.SearchAsync(searchFilter);

            return matchingUsers.FirstOrDefault();
        }
    }
}
