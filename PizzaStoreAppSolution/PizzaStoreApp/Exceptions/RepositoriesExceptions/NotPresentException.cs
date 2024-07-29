using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class NotPresentException : Exception
    {
        public NotPresentException()
        {
        }

        public NotPresentException(string? message) : base(message)
        {
        }

        public NotPresentException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotPresentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}