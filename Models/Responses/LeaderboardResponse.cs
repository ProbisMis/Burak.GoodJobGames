﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.Responses
{
    public class LeaderboardResponse
    {
        public int Score { get; set; }
        public Guid UserId { get; set; }
        public int Rank { get; set; }
        public string CountryName { get; set; }
        public string Key { get; set; }
    }
}
