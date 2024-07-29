using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    internal class BeverageNotFoundException : Exception
    {
        public BeverageNotFoundException()
        {
        }

        public BeverageNotFoundException(string? message) : base(message)
        {
        }

        public BeverageNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BeverageNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}