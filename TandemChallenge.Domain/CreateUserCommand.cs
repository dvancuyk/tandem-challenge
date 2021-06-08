using MediatR;

namespace TandemChallenge.Domain
{
    /// <summary>
    /// Encapsulates the information needed to request a new user to be added to the system.
    /// </summary>
    public class CreateUserCommand : IRequest<User>
    {
        public CreateUserCommand(string firstName, string middleName, string lastName, string phoneNumber, string emailAddress)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public string FirstName { get; private set; }

        public string MiddleName { get; private set; }

        public string LastName { get; private set; }
        
        public string PhoneNumber { get; private set; }

        public string EmailAddress { get; private set; }
    }
}
