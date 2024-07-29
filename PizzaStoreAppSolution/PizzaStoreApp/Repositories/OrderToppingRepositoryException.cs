using System.Runtime.Serialization;

namespace PizzaStoreApp.Repositories
{
    [Serializable]
    internal class OrderToppingRepositoryException : Exception
    {
        public OrderToppingRepositoryException()
        {
        }

        public OrderToppingRepositoryException(string? message) : base(message)
        {
        }

        public OrderToppingRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderToppingRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}