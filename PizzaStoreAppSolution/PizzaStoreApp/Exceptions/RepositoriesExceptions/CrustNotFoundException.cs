using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CrustNotFoundException : Exception
    {
        public CrustNotFoundException()
        {
        }

        public CrustNotFoundException(string? message) : base(message)
        {
        }

        public CrustNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CrustNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}