using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class PaymentRepositoryException : Exception
    {
        public PaymentRepositoryException()
        {
        }

        public PaymentRepositoryException(string? message) : base(message)
        {
        }

        public PaymentRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PaymentRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}