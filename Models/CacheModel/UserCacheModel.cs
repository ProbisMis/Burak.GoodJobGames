using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.CacheModel
{
    public class UserCacheModel
    {
        public Guid GID { get; set; }
        public string Username { get; set; }
        public string CountryIsoCode { get; set; }
    }
}
