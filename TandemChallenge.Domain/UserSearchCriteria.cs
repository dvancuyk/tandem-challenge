namespace TandemChallenge.Domain
{
    /// <summary>
    /// Contains the different search criteria that can be used to search for users within the system.
    /// </summary>
    public class UserSearchCriteria
    {
        public UserSearchCriteria(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Searches for users that have the provided email.
        /// </summary>
        public string Email { get; }
    }
}
