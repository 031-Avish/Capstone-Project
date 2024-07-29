using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.ServiceException
{
    [Serializable]
    internal class BeverageServiceException : Exception
    {
        public BeverageServiceException()
        {
        }

        public BeverageServiceException(string? message) : base(message)
        {
        }

        public BeverageServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BeverageServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}