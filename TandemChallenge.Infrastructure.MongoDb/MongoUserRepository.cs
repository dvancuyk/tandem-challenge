using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TandemChallenge.Domain;

namespace TandemChallenge.Infrastructure.MongoDb
{
    /// <summary>
    /// Implementation of <see cref="IUserRepository"/> which uses a CosmosDB mongo DB backend store as the repository.
    /// </summary>
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> collection;

        public MongoUserRepository(IOptions<MongoConnection> options)
        {
            if(options?.Value == null)
            {
                throw new ArgumentNullException("The MongoConnection values must be provided.");
            }

            var connectionSettings = options.Value;
            var client = new MongoClient(connectionSettings.ConnectionString);
            var database = client.GetDatabase(connectionSettings.Database);
            this.collection = database.GetCollection<User>(nameof(User));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> SearchAsync(UserSearchCriteria searchCriteria)
        {
            var filterBuilder = Builders<User>.Filter;
            var filter = filterBuilder.Empty;

            if(!string.IsNullOrEmpty(searchCriteria.Email))
            {
                filter &= filterBuilder.Eq(u => u.EmailAddress, searchCriteria.Email);
            }

            var records = await collection
            .FindAsync(filter);

            return records.ToList();
        }

        /// <inheritdoc />
        public async Task AddAsync(User user)
        {
            await collection.InsertOneAsync(user);
        }

        /// <summary>
        /// Retrieves the user with the provided id.
        /// </summary>
        /// <param name="id">The system-generated identifier for the user</param>
        /// <returns></returns>
        public async Task<User> GetAsync(Guid id)
        {
            var records = await collection
                .FindAsync(r => r.Id == id);

            var record = records.SingleOrDefault();

            return record;
        }
    }
}
