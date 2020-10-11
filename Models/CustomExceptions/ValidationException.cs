using System;

namespace Burak.GoodJobGames.Models.CustomExceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, Exception innerEx = null) : base(message, innerEx)
        {
        }
    }
}