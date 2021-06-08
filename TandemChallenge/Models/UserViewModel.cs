using System;

namespace TandemChallenge.Api.Models
{
    /// <summary>
    /// Represents a user within the system.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// The internal identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The full name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The primary phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The primary email address.
        /// </summary>
        public string EmailAddress { get; set; }
    }

}
