using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class UserServiceException : Exception
    {
        public UserServiceException()
        {
        }

        public UserServiceException(string? message) : base(message)
        {
        }

        public UserServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}