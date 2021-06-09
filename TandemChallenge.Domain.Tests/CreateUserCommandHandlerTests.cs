using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;

namespace TandemChallenge.Domain.Tests
{
    [TestClass, TestCategory("unit")]
    public class CreateUserCommandHandlerTests
    {
        private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();
        private readonly ILogger<CreateUserCommandHandler> logger = Substitute.For<ILogger<CreateUserCommandHandler>>();
        private readonly IMapper mapper = Substitute.For<IMapper>();

        [TestInitialize]
        public void Initialize()
        {
            userRepository.ClearReceivedCalls();
            mapper
                .Map<User>(Arg.Any<CreateUserCommand>())
                .Returns(ci =>
                {
                    var command = ci.Arg<CreateUserCommand>();
                    return new User()
                    {
                        FirstName = command.FirstName,
                        MiddleName = command.MiddleName,
                        LastName = command.LastName,
                        PhoneNumber = command.PhoneNumber,
                        EmailAddress = command.EmailAddress,
                    };
                });
        }

        [TestMethod]
        public async Task HandleShould_CreateUser_GivenEmailIsUnique()
        {
            // Arrange
            var handler = new CreateUserCommandHandler(userRepository, mapper, logger);

            var command = new CreateUserCommand("Jane", "Lee", "Doe", "555-123-9876", "jdoe@fakedomain.com");

            // Act 
            var user = await handler.Handle(command, CancellationToken.None);

            // Assert
            user.Should().NotBeNull();
            await userRepository.Received(1).AddAsync(user);
        }
    }
}
