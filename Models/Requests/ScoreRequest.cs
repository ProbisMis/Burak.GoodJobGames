﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Models.Requests
{
    public class ScoreRequest
    {
        public int UserId { get; set; }
        public int UserScore { get; set; }
    }
}