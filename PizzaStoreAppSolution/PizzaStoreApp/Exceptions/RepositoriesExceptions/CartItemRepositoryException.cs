using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CartItemRepositoryException : Exception
    {
        public CartItemRepositoryException()
        {
        }

        public CartItemRepositoryException(string? message) : base(message)
        {
        }

        public CartItemRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartItemRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}