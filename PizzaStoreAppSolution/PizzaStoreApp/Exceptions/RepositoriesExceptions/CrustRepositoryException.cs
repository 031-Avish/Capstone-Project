using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class CrustRepositoryException : Exception
    {
        public CrustRepositoryException()
        {
        }

        public CrustRepositoryException(string? message) : base(message)
        {
        }

        public CrustRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CrustRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}