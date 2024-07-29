using System.Runtime.Serialization;

namespace PizzaStoreApp.Repositories
{
    [Serializable]
    internal class OrderDetailNotFoundException : Exception
    {
        public OrderDetailNotFoundException()
        {
        }

        public OrderDetailNotFoundException(string? message) : base(message)
        {
        }

        public OrderDetailNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderDetailNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}