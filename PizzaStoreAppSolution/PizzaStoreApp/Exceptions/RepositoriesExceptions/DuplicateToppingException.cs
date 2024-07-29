using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class DuplicateToppingException : Exception
    {
        public DuplicateToppingException()
        {
        }

        public DuplicateToppingException(string? message) : base(message)
        {
        }

        public DuplicateToppingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateToppingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}