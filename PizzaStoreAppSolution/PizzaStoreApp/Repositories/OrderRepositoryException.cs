using System.Runtime.Serialization;

namespace PizzaStoreApp.Repositories
{
    [Serializable]
    internal class OrderRepositoryException : Exception
    {
        public OrderRepositoryException()
        {
        }

        public OrderRepositoryException(string? message) : base(message)
        {
        }

        public OrderRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}