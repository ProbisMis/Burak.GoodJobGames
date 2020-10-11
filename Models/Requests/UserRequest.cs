using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Models.Requests
{
    public class UserRequest
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } //TODO: Convert to Password Model (hash,password,salt,updated,userid,id), Map to Passwords
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
    }
}
