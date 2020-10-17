using GoodJobGames.Models;
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
        void SortedSetAddRange<T>(string key, List<T> data);
        List<LeaderboardCacheModel> SortedSetGetAll(string key);
        void Remove(string key);
        void Clear();
        bool hasAny(string key);
        Task<int> SortedSetGetScore(string key, LeaderboardCacheModel data);
        Task<int> SortedSetGetRank(string key, LeaderboardCacheModel data);
        Task SortedSetAdd(string key, int score, LeaderboardCacheModel data);
        Task SortedSetIncrement(string key, int score, LeaderboardCacheModel data);
        Task<string> SortedSetGetAllCacheModel(string key);
        Task<string> SortedSetIntersect(string firstKey, string secondKey, string destination);

        Task<Dictionary<string,string>> HashGetAll(Guid key);
        void HashSet(UserCacheModel model);
        Task<string> HashGet(UserCacheModel model, string propertyName);
        Task<bool> IsHashExist(Guid key);
    }
}
