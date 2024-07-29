using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.ServiceException
{
    [Serializable]
    internal class PizzaServiceException : Exception
    {
        public PizzaServiceException()
        {
        }

        public PizzaServiceException(string? message) : base(message)
        {
        }

        public PizzaServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PizzaServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}