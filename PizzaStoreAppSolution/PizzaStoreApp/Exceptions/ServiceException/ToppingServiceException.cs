using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.ServiceException
{
    [Serializable]
    internal class ToppingServiceException : Exception
    {
        public ToppingServiceException()
        {
        }

        public ToppingServiceException(string? message) : base(message)
        {
        }

        public ToppingServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ToppingServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}