using System.Runtime.Serialization;

namespace PizzaStoreApp.Services
{
    [Serializable]
    internal class BlobServiceException : Exception
    {
        public BlobServiceException()
        {
        }

        public BlobServiceException(string? message) : base(message)
        {
        }

        public BlobServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BlobServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}