using System;

namespace Burak.GoodJobGames.Models.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, Exception innerEx = null) : base(message, innerEx)
        {
        }
    }
}