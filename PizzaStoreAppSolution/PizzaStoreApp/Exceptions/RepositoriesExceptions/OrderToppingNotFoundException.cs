using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class OrderToppingNotFoundException : Exception
    {
        public OrderToppingNotFoundException()
        {
        }

        public OrderToppingNotFoundException(string? message) : base(message)
        {
        }

        public OrderToppingNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderToppingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}