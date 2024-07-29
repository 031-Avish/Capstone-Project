using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class ToppingNotFoundException : Exception
    {
        public ToppingNotFoundException()
        {
        }

        public ToppingNotFoundException(string? message) : base(message)
        {
        }

        public ToppingNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ToppingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}