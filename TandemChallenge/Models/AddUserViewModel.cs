namespace TandemChallenge.Api.Models
{
    /// <summary>
    /// Defines the contract for adding a new user within the system.
    /// </summary>
    public class AddUserViewModel
    {
        /// <summary>
        /// The first name of the user to be added.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The middle name of the user to be added.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The last name of the user to be added.
        /// </summary>
        public string LastName { get; set; }
        
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
