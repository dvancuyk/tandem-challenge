using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TandemChallenge.Domain.Tests
{
    [TestClass, TestCategory("unit")]
    public class ValidateCreateUserCommandTests
    {
        private bool nextCalled = false;
        private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();
        private Task<User> DefaultNextOperation() 
        {
            nextCalled = true;
            return Task.FromResult((User)null); 
        }

        [TestInitialize]
        public void Initialize()
        {
            nextCalled = false;
            userRepository.ClearReceivedCalls();
        }

        [TestMethod]
        public async Task Handle_ShouldProcessPipeline_GivenCommandIsValid()
        {
            // Arrange
            var command = new CreateUserCommand("Test", "A.", "User", "555-123-9876", "test.a.user@fakedomain.com");

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            await validator.Handle(command, CancellationToken.None, DefaultNextOperation);

            // Assert
            nextCalled.Should().BeTrue();
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
            // Arrange
            var command = new CreateUserCommand("Test", "A.", "User", "555-123-9876", null);

            var validator = new ValidateCreateUserCommand(userRepository);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(() => validator.Handle(command, CancellationToken.None, DefaultNextOperation));

            // Assert
            exception.Errors.Should()
                .HaveCount(1)
                .And
                .Contain(ve => ve.PropertyName == "EmailAddress" && ve.ErrorMessage == "Email address is required.");
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
                .Contain(ve => ve.PropertyName == "EmailAddress" && ve.ErrorMessage == "A valid email address must be provided");
        }

        [TestMethod]
        public async Task Handle_ShouldThrowValidationException_GivenPhoneNumberIsNotValid()
        {
            // Arrange
            var command = new CreateUserCommand("Test", "A.", "User", "abd-123-9876", "test.a.user@fakedomain.com");
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
