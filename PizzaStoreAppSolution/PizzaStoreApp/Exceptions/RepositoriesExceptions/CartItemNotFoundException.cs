using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CartItemNotFoundException : Exception
    {
        public CartItemNotFoundException()
        {
        }

        public CartItemNotFoundException(string? message) : base(message)
        {
        }

        public CartItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}