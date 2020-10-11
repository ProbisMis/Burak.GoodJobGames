using System;

namespace Burak.GoodJobGames.Models.CustomExceptions
{
    public class IntegrationException : Exception
    {
        public IntegrationException(string message, Exception innerEx = null) : base(message, innerEx)
        {
        }
    }
}