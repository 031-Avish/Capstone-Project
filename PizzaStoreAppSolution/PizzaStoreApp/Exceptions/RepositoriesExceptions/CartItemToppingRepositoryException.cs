using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CartItemToppingRepositoryException : Exception
    {
        public CartItemToppingRepositoryException()
        {
        }

        public CartItemToppingRepositoryException(string? message) : base(message)
        {
        }

        public CartItemToppingRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartItemToppingRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}