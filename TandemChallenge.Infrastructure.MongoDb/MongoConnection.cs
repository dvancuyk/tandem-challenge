namespace TandemChallenge.Infrastructure.MongoDb
{
    /// <summary>
    /// Encapsulates information to connect to a specific datastore. 
    /// </summary>
    public class MongoConnection
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }
    }
}
