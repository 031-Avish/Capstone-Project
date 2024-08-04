using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class OrderDetailRepositoryException : Exception
    {
        public OrderDetailRepositoryException()
        {
        }

        public OrderDetailRepositoryException(string? message) : base(message)
        {
        }

        public OrderDetailRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderDetailRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}