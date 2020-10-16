using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.Requests
{
    public class ScoreRequest
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
    }
}
