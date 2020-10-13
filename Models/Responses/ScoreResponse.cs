using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.Responses
{
    public class ScoreResponse
    {
        public int Score { get; set; }
        public Guid UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
