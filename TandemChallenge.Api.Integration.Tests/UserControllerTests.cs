using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TandemChallenge.Api.Models;

namespace TandemChallenge.Api.Integration.Tests
{
    [TestClass, TestCategory("integration")]
    public class UserControllerTests
    {
		private WebApplicationFactory<Startup> webAppFactory;

		[TestInitialize]
		public void SetupFixture()
		{
			webAppFactory = new WebApplicationFactory<Startup>()
				.WithWebHostBuilder(
					builder =>
					{
						builder.ConfigureServices(services => { });
						builder.UseEnvironment("tests");
					});

		}

		private static AddUserViewModel GenerateUser()
		{
			var random = new Random();
			var firstName = "Test" + random.Next();
			var lastName = "User" + random.Next();

			return new AddUserViewModel
			{
				FirstName = firstName,
				MiddleName = "T.",
				LastName = lastName,
				EmailAddress = $"{firstName}.{lastName}@fakeaccount.com",
				PhoneNumber = "555-123-9876"
			};
		}

		[TestMethod]
		public async Task Add_ShouldReturnUser_GivenUserEmailDoesExist()
		{
			using (var httpClient = webAppFactory.CreateClient())
			{
				var user = GenerateUser();
				var response = await httpClient.PostAsJsonAsync("/User", user);
				response.StatusCode.Should().Be(HttpStatusCode.OK);
			}
		}

		[TestMethod]
		public async Task Add_ShouldReturnBadRequest_GivenUserEmailAlreadyExist()
		{
			using (var httpClient = webAppFactory.CreateClient())
			{
				var user = GenerateUser();
				await httpClient.PostAsJsonAsync("/User", user);

				var response = await httpClient.PostAsJsonAsync("/User", user);
				response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			}
		}

		[TestMethod]
		public async Task Add_ShouldReturnBadRequest_GivenUserFirstNameIsMissing()
		{
			using (var httpClient = webAppFactory.CreateClient())
			{
				var user = GenerateUser();
				await httpClient.PostAsJsonAsync("/User", user);

				var response = await httpClient.PostAsJsonAsync("", user);
				response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			}
		}

		[TestMethod]
        public async Task Get_ShouldReturnNotFound_GivenEmailDoesNotExist()
        {
			using (var httpClient = webAppFactory.CreateClient())
			{
				var response = await httpClient.GetAsync("User/not.found@fakedomain.com");
				response.StatusCode.Should().Be(HttpStatusCode.NotFound);
			}
		}

        [TestMethod]
        public async Task Get_ShouldReturnUser_GivenUserEmailDoesExist()
        {
			using (var httpClient = webAppFactory.CreateClient())
			{
				var user = GenerateUser();
				await httpClient.PostAsJsonAsync("/User", user);

				var response = await httpClient.GetAsync(user.EmailAddress);
				response.StatusCode.Should().Be(HttpStatusCode.OK);
			}
		}
    }
}
