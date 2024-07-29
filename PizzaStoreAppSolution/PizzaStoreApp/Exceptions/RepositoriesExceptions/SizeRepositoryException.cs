using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class SizeRepositoryException : Exception
    {
        public SizeRepositoryException()
        {
        }

        public SizeRepositoryException(string? message) : base(message)
        {
        }

        public SizeRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SizeRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}