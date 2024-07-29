using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class SizeNotFoundException : Exception
    {
        public SizeNotFoundException()
        {
        }

        public SizeNotFoundException(string? message) : base(message)
        {
        }

        public SizeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SizeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}