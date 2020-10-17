using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.Requests.Import
{
    public class BulkImportUserRequest
    {
        public int NumberOfUsers { get; set; }
        public string UsernamePrefix { get; set; }
        public string FixedPassword { get; set; }

    }
}
