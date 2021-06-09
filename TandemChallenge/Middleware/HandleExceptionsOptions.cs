namespace TandemChallenge.Api.Middleware
{
    /// <summary>
    /// Configuration to change how the <see cref="HandleExceptionsMiddleware"/> should work.
    /// </summary>
    public class HandleExceptionsOptions
    {
        /// <summary>
        /// Creates a new instance of the <see cref="HandleExceptionsOptions"/>
        /// </summary>
        /// <param name="rules"></param>
        public HandleExceptionsOptions(ExceptionRule[] rules)
        {
            Rules = rules;
        }

        /// <summary>
        /// The list of rules the <see cref="HandleExceptionsMiddleware"/> should use when handling exceptions
        /// </summary>
        public ExceptionRule[] Rules { get; }
    }
}