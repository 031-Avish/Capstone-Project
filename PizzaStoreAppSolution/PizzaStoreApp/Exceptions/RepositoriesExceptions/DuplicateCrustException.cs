using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class DuplicateCrustException : Exception
    {
        public DuplicateCrustException()
        {
        }

        public DuplicateCrustException(string? message) : base(message)
        {
        }

        public DuplicateCrustException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateCrustException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}