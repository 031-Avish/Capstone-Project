using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class PizzaNotFoundException : Exception
    {
        public PizzaNotFoundException()
        {
        }

        public PizzaNotFoundException(string? message) : base(message)
        {
        }

        public PizzaNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PizzaNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}