using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Models.CustomExceptions
{
    public class ServiceAdaptorException
    {
        /// <summary>
        /// Error code for service owners. We do not need any error code for response, but
        /// it is easy to determine where the error occures when there is a code for it.
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Error message for the return value. If UserFriendly value is true then we print
        /// this message to user else we print a default error message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Determines if the error message will print to screen or not.
        /// </summary>
        public bool UserFriendly { get; set; }
    }
}
