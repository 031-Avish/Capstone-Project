using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class BeverageRepositoryException : Exception
    {
        public BeverageRepositoryException()
        {
        }

        public BeverageRepositoryException(string? message) : base(message)
        {
        }

        public BeverageRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BeverageRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}