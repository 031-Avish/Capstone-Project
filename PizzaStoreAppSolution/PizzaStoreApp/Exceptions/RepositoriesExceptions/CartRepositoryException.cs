using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CartRepositoryException : Exception
    {
        public CartRepositoryException()
        {
        }

        public CartRepositoryException(string? message) : base(message)
        {
        }

        public CartRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}