namespace TandemChallenge.Domain.Tests
{
    internal class UserBuilder
    {
        public static User CreateUser(string firstName, string lastName, string middleName = null)
        {
            return new User
            {
                EmailAddress = $"{firstName}.{lastName}@fakedomain.com",
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = "555-123-9876"
            };
        }
    }
}
