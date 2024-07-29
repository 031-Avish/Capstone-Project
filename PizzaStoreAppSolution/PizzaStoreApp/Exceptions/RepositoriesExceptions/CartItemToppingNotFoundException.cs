using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CartItemToppingNotFoundException : Exception
    {
        public CartItemToppingNotFoundException()
        {
        }

        public CartItemToppingNotFoundException(string? message) : base(message)
        {
        }

        public CartItemToppingNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartItemToppingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}