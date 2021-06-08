using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;

namespace TandemChallenge.Domain.Tests
{
    [TestClass, TestCategory("unit")]
    public class FindUserByEmailHandlerTests
    {
        private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();
        private readonly ILogger<FindUserByEmailHandler> logger = Substitute.For<ILogger<FindUserByEmailHandler>>();

        [TestInitialize]
        public void Initialize()
        {
            userRepository.ClearReceivedCalls();
        }

        [TestMethod]
        public async Task HandleShould_ReturnUser_GivenUserWithEmailExists()
        {
            // Arrange
            var user = UserBuilder.CreateUser("John", "Doe");
            userRepository
                .SearchAsync(Arg.Is<UserSearchCriteria>(filter => filter.Email == user.EmailAddress))
                .Returns(new[] { user });

            var handler = new FindUserByEmailHandler(userRepository, logger);

            var command = new FindUserByEmailQuery(user.EmailAddress);

            // Act 
            var foundUser = await handler.Handle(command, CancellationToken.None);

            // Assert
            foundUser.Should().Be(user);
        }

        [TestMethod]
        public async Task HandleShould_CreateUser_GivenEmailIsUnique()
        {
            // Arrange
            var handler = new FindUserByEmailHandler(userRepository, logger);

            var command = new FindUserByEmailQuery("jdoe@fakedomain.com");

            // Act 
            var user = await handler.Handle(command, CancellationToken.None);

            // Assert
            user.Should().BeNull();
        }
    }
}
