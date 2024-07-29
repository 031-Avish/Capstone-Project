using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    public class DuplicatePizzaException : Exception
    {
        public DuplicatePizzaException()
        {
        }

        public DuplicatePizzaException(string? message) : base(message)
        {
        }

        public DuplicatePizzaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}