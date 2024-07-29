using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class DuplicateSizeException : Exception
    {
        public DuplicateSizeException()
        {
        }

        public DuplicateSizeException(string? message) : base(message)
        {
        }

        public DuplicateSizeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateSizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}