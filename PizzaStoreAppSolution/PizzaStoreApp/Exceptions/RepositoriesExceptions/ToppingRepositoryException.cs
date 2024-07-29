using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class ToppingRepositoryException : Exception
    {
        public ToppingRepositoryException()
        {
        }

        public ToppingRepositoryException(string? message) : base(message)
        {
        }

        public ToppingRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ToppingRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}