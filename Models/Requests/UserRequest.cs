using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.Requests
{
    public class UserRequest
    {
        public Guid GID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CountryIsoCode { get; set; }
    }
}
