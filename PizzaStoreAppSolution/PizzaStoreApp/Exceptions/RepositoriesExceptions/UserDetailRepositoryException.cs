﻿using System.Runtime.Serialization;

namespace PizzaStoreApp.Exceptions.RepositoriesExceptions
{
    [Serializable]
    public class UserDetailRepositoryException : Exception
    {
        public UserDetailRepositoryException()
        {
        }

        public UserDetailRepositoryException(string? message) : base(message)
        {
        }

        public UserDetailRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        protected UserDetailRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}