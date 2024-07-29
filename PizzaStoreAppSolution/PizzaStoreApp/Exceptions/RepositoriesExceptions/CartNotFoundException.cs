using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CartNotFoundException : Exception
    {
        public CartNotFoundException()
        {
        }

        public CartNotFoundException(string? message) : base(message)
        {
        }

        public CartNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}