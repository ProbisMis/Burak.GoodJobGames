﻿using GoodJobGames.Models;
using GoodJobGames.Models.CacheModel;
using GoodJobGames.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Services.Interface
{
    public interface ICacheService
    {
        void Remove(string key);
        void Clear();
        bool hasAny(string key);

        /* SORTED SET*/
        Task<int> SortedSetGetScore(string key, LeaderboardCacheModel data);
        Task<int> SortedSetGetRank(string key, LeaderboardCacheModel data);
        Task SortedSetAdd(string key, int score, LeaderboardCacheModel data);
        Task SortedSetIncrement(string key, int score, LeaderboardCacheModel data);
        Task<string> SortedSetIntersect(string firstKey, string secondKey, string destination);
        List<LeaderboardCacheModel> SortedSetGetAll(string key, int pageNumber);
        Task<bool> hasAny(string key, LeaderboardCacheModel data);


        /* HASH */
        Task<Dictionary<string,string>> HashGetAll(Guid key);
        void HashSet(UserCacheModel model);
        Task<string> HashGet(UserCacheModel model, string propertyName);
        Task<bool> IsHashExist(Guid key);
    }
}
