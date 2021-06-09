using FluentAssertions;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TandemChallenge.Domain.Tests
{
    [TestClass, TestCategory("unit")]
    public class ValidateCreateUserCommandTests
    {
        private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();
        private Task<User> DefaultNextOperation() { return Task.FromResult((User)null); }

        [TestInitialize]
        public void Initialize()
        {
            userRepository.ClearReceivedCalls();
        }

        [TestMethod]
        public async Task Handle_ShouldThrowValidationException_GivenEmailAlreadyExists()
        {
            // Arrange
            var command = new CreateUserCommand("Test", "A.", "User", "555-123-9876", "test.a.user@fakedomain.com");
            userRepository
                .SearchAsync(Arg.Is<UserSearchCriteria>(filter => filter.Email == command.EmailAddress))
                .Returns(new[] { new User() });

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(() => validator.Handle(command, CancellationToken.None, DefaultNextOperation));

            // Assert
            exception.Errors.Should()
                .HaveCount(1)
                .And
                .Contain(ve => ve.PropertyName == "EmailAddress" && ve.ErrorMessage == $"The email '{command.EmailAddress}' has already been registered.");
        }

        [TestMethod]
        public async Task Handle_ShouldThrowValidationException_GivenFirstNameIsMissing()
        {
            // Arrange
            var command = new CreateUserCommand(string.Empty, "A.", "User", "555-123-9876", "test.a.user@fakedomain.com");

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(() => validator.Handle(command, CancellationToken.None, DefaultNextOperation));

            // Assert
            exception.Errors.Should()
                .HaveCount(1)
                .And
                .Contain(ve => ve.PropertyName == "FirstName");
        }

        [TestMethod]
        public async Task Handle_ShouldThrowValidationException_GivenLastNameIsMissing()
        {
            // Arrange
            var command = new CreateUserCommand("Test", "A.", null, "555-123-9876", "test.a.user@fakedomain.com");
            userRepository
                .SearchAsync(Arg.Is<UserSearchCriteria>(filter => filter.Email == command.EmailAddress))
                .Returns(new[] { new User() });

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(() => validator.Handle(command, CancellationToken.None, DefaultNextOperation));

            // Assert
            exception.Errors.Should()
                .HaveCount(1)
                .And
                .Contain(ve => ve.PropertyName == "LastName");
        }

        [TestMethod]
        public async Task Handle_ShouldThrowValidationException_GivenEmailIsMissing()
        {
            var command = new CreateUserCommand("Test", "A.", "User", "555-123-9876", null);
            userRepository
                .SearchAsync(Arg.Is<UserSearchCriteria>(filter => filter.Email == command.EmailAddress))
                .Returns(new[] { new User() });

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(() => validator.Handle(command, CancellationToken.None, DefaultNextOperation));

            // Assert
            exception.Errors.Should()
                .HaveCount(1)
                .And
                .Contain(ve => ve.PropertyName == "EmailAddress" && ve.ErrorMessage == "Email is required.");
        }

        [TestMethod]
        public async Task Handle_ShouldThrowValidationException_GivenEmailIsNotValid()
        {
            var command = new CreateUserCommand("Test", "A.", "User", "555-123-9876", "test.a.user-fakedomain.com");
            userRepository
                .SearchAsync(Arg.Is<UserSearchCriteria>(filter => filter.Email == command.EmailAddress))
                .Returns(new[] { new User() });

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(() => validator.Handle(command, CancellationToken.None, DefaultNextOperation));

            // Assert
            exception.Errors.Should()
                .HaveCount(1)
                .And
                .Contain(ve => ve.PropertyName == "EmailAddress" && ve.ErrorMessage == "Email is not in the correct format");
        }

        [TestMethod]
        public async Task Handle_ShouldThrowValidationException_GivenPhoneNumberIsNotValid()
        {
            var command = new CreateUserCommand("Test", "A.", "User", "abd-123-9876", "test.a.user@fakedomain.com");
            userRepository
                .SearchAsync(Arg.Is<UserSearchCriteria>(filter => filter.Email == command.EmailAddress))
                .Returns(new[] { new User() });

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(() => validator.Handle(command, CancellationToken.None, DefaultNextOperation));

            // Assert
            exception.Errors.Should()
                .HaveCount(1)
                .And
                .Contain(ve => ve.PropertyName == "PhoneNumber");
        }
    }
}
