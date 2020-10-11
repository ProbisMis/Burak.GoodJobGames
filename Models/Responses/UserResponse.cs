using Burak.GoodJobGames.Models.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Models.Responses
{
    public class UserResponse : ServiceAdaptorException
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        public string Token { get; set; }
    }
}
