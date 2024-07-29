using System.Runtime.Serialization;

namespace PizzaStoreApp.Services
{
    [Serializable]
    internal class CartServiceException : Exception
    {
        public CartServiceException()
        {
        }

        public CartServiceException(string? message) : base(message)
        {
        }

        public CartServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}