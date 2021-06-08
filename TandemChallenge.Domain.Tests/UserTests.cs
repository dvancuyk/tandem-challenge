using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TandemChallenge.Domain.Tests
{
    [TestClass, TestCategory("unit")]
    public class UserTests
    {

        [TestMethod]
        public void FullName_ShouldReturnFullName_GivenAllPartsExist()
        {
            // Arrange
            var user = UserBuilder.CreateUser("Test", "User", "QA");

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be($"{user.FirstName} {user.MiddleName} {user.LastName}");
        }

        [TestMethod]
        public void FullName_ShouldReturnFirstAndLastNameOnly_GivenMiddleNameIsNull()
        {
            // Arrange
            var user = UserBuilder.CreateUser("Test", "User", null);

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be($"{user.FirstName} {user.LastName}");
        }

        [TestMethod]
        public void FullName_ShouldReturnFirstAndLastNameOnly_GivenMiddleNameIsWhiteSpace()
        {
            // Arrange
            var user = UserBuilder.CreateUser("Test", "User", " ");

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be($"{user.FirstName} {user.LastName}");
        }
    }
}
