using System.Collections.Generic;
using System.Threading.Tasks;

namespace TandemChallenge.Domain
{
    /// <summary>
    /// Defines the contract for retrieving users from a persistent store.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Saves the provided user.
        /// </summary>
        /// <param name="user">The user that needs to be saved.</param>
        Task AddAsync(User user);
        /// <summary>
        /// Finds a set of users that match all of the provided options.
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns>A collection of users if any match the provided search criteria</returns>
        Task<IEnumerable<User>> SearchAsync(UserSearchCriteria searchCriteria);
    }
}
