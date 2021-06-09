using System;
using System.Net;

namespace TandemChallenge.Api.Middleware
{
    /// <summary>
    /// Defines a rule for a status type that should be returned when an exception of the specified type is encountered.
    /// </summary>
    public class ExceptionRule
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ExceptionRule"/>
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="expectedReturnStatus"></param>
        public ExceptionRule(Type exceptionType, HttpStatusCode expectedReturnStatus)
        {
            ExceptionType = exceptionType;
            ExpectedReturnStatus = expectedReturnStatus;
        }

        /// <summary>
        /// The type of exception this rule should handle.
        /// </summary>
        public Type ExceptionType { get; }

        /// <summary>
        /// The status that should be returned when this exception type is encountered.
        /// </summary>
        public HttpStatusCode ExpectedReturnStatus { get; }

        /// <summary>
        /// Creates a new rule for the indicated exception by returning the provided error code
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="returnCode"></param>
        /// <returns></returns>
        public static ExceptionRule For<TException>(HttpStatusCode returnCode)
            where TException : Exception
        {
            return new ExceptionRule(typeof(TException), returnCode);
        }
    }
}
