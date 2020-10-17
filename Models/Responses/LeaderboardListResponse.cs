using GoodJobGames.Models.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.Responses
{
    public class LeaderboardListResponse : ServiceAdaptorException
    {
        public List<LeaderboardResponse> LeaderboardResponses { get; set; }
    }
}
