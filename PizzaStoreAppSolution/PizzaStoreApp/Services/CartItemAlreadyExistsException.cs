using System.Runtime.Serialization;

namespace PizzaStoreApp.Services
{
    [Serializable]
    internal class CartItemAlreadyExistsException : Exception
    {
        public CartItemAlreadyExistsException()
        {
        }

        public CartItemAlreadyExistsException(string? message) : base(message)
        {
        }

        public CartItemAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartItemAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}