using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class DuplicateBeverageException : Exception
    {
        public DuplicateBeverageException()
        {
        }

        public DuplicateBeverageException(string? message) : base(message)
        {
        }

        public DuplicateBeverageException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateBeverageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}