using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{

    public class PizzaRepositoryException : Exception
    {
        public PizzaRepositoryException()
        {
        }

        public PizzaRepositoryException(string? message) : base(message)
        {
        }

        public PizzaRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }


        protected PizzaRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}