using System;
using System.Collections;
using System.Text;
using System.Xml.Linq;

namespace TandemChallenge.Domain
{
    /// <summary>
    /// Represents a user within the system.
    /// </summary>
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public User(Guid id, string firstName, string middleName, string lastName, string phoneNumber, string emailAddress)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public Guid Id { get; private set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string FullName
        {
            get
            {
                var name = new StringBuilder(FirstName);
                if (!string.IsNullOrWhiteSpace(MiddleName))
                {
                    name.Append(" ");
                    name.Append(MiddleName);
                }
                name.Append(" ");
                name.Append(LastName);

                return name.ToString();
            }
        }
    }
}
