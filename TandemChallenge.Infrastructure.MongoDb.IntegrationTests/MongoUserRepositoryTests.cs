using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
using TandemChallenge.Domain;

namespace TandemChallenge.Infrastructure.MongoDb.IntegrationTests
{
    [TestClass]
    public class MongoUserRepositoryTests
    {
        private static IOptions<MongoConnection> options;

        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var mongoSettings = new MongoConnection
            {
                ConnectionString = config["mongoConnection"],
                Database = config["database"]
            };

            options = Substitute.For<IOptions<MongoConnection>>();
            options.Value.Returns(mongoSettings);
        }

        private static User GenerateUser()
        {
            var random = new Random();
            var firstName = "Test" + random.Next();
            var lastName = "User" + random.Next();

            return new User
            {
                FirstName = firstName,
                MiddleName = "T.",
                LastName = lastName,
                EmailAddress = $"{firstName}.{lastName}@fakeaccount.com",
                PhoneNumber = "555-123-9876"
            };
        }

        [TestMethod, TestCategory("integration")]
        public async Task AddAsync_ShouldCreateUser_GivenItDoesNotExist()
        {
            // Arrange
            var user = GenerateUser();
            var repository = new MongoUserRepository(options);

            // Act
            await repository.AddAsync(user);

            // Assert
            var fetchedUser = await repository.GetAsync(user.Id);

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(user.Id);
            fetchedUser.FirstName.Should().Be(user.FirstName);
            fetchedUser.MiddleName.Should().Be(user.MiddleName);
            fetchedUser.LastName.Should().Be(user.LastName);
            fetchedUser.PhoneNumber.Should().Be(user.PhoneNumber);
            fetchedUser.EmailAddress.Should().Be(user.EmailAddress);
        }

        [TestMethod, TestCategory("integration")]
        public async Task SearchAsync_ShouldReturnEmptyCollection_GivenEmailDoesNotMatchAnyRecords()
        {
            // Arrange
            var repository = new MongoUserRepository(options);
            var filter = new UserSearchCriteria("does.not.exist@anotherfakedomain.com");

            // Act
            var records = await repository.SearchAsync(filter);

            // Assert
            records.Should().BeEmpty();
        }

        [TestMethod, TestCategory("integration")]
        public async Task SearchAsync_ShouldReturnPopulatedCollection_GivenEmailMatchesExistingRecords()
        {
            // Arrange
            var user = GenerateUser();
            var repository = new MongoUserRepository(options);
            await repository.AddAsync(user);
            var filter = new UserSearchCriteria(user.EmailAddress);

            // Act
            var records = await repository.SearchAsync(filter);

            // Assert
            records.Should().NotBeEmpty()
                .And
                .ContainSingle(u => u.EmailAddress == user.EmailAddress);
        }
    }
}
