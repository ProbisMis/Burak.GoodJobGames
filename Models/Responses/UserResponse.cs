﻿using GoodJobGames.Models.CustomExceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Models.Responses
{
    public class UserResponse  : ServiceAdaptorException
    {
        public Guid GID { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        [JsonIgnore]
        public string CountryIsoCode { get; set; }
    }
}
