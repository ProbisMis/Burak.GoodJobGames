using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data;
using GoodJobGames.Models.CacheModel;
using GoodJobGames.Models.CustomExceptions;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Utilities.Helper;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Services.Implementation
{
    public class RedisCacheService : ICacheService
    {
        private readonly RedisServer _redisServer;
        public RedisCacheService(RedisServer redisServer)
        {
            _redisServer = redisServer;
        }
        public void Clear()
        {
            _redisServer.FlushDatabase();
        }

        public void Remove(string key)
        {
            _redisServer.Database.KeyDelete(key);
        }

        public async Task SortedSetAdd(string key, int score, LeaderboardCacheModel data)
        {
            await _redisServer.Database.SortedSetAddAsync(key, JsonConvert.SerializeObject(data), score);
        }

        public async Task SortedSetIncrement(string key, int score, LeaderboardCacheModel data)
        {
            await _redisServer.Database.SortedSetIncrementAsync(key, JsonConvert.SerializeObject(data), score);
            //Increment on Global Cache
            await _redisServer.Database.SortedSetIncrementAsync(CacheKeyConstants.ALL_LEADERBOARD_KEY, JsonConvert.SerializeObject(data), score);
        }

        public async Task<bool> hasAny(string key, LeaderboardCacheModel data)
        {
            var x = await _redisServer.Database.SortedSetRankAsync(key, JsonConvert.SerializeObject(data), Order.Descending);
            if (x.HasValue)
                return true;
            return false;
        }
        
        public async Task<int> SortedSetGetRank(string key, LeaderboardCacheModel data)
        {
            var x = await _redisServer.Database.SortedSetRankAsync(key, JsonConvert.SerializeObject(data), Order.Descending);
            if (x.HasValue)
            {
                return Convert.ToInt32(x.Value) + 1;
            }
            return -1;
        }

        public async Task<int> SortedSetGetScore(string key, LeaderboardCacheModel data)
        {
            var x = await _redisServer.Database.SortedSetScoreAsync(key, JsonConvert.SerializeObject(data));
            if (x.HasValue)
            {
                return Convert.ToInt32(x.Value);
            }
            return -1;
        }

        public async Task<string> SortedSetIntersect(string firstKey, string secondKey, string destination)
        {
            var x = await _redisServer.Database.SortedSetCombineAndStoreAsync(SetOperation.Intersect, destination, firstKey, secondKey);
            return x.ToString();
        }

        public List<LeaderboardCacheModel> SortedSetGetAll(string key, int pageNumber)
        {
            try
            {
                RedisValue[] values = _redisServer.Database.SortedSetRangeByRank(key, (pageNumber-1)*100, pageNumber*100, Order.Descending);
                return CacheUtils.ToListLeaderboardCacheModel(values);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public bool hasAny(string key)
        {
            return _redisServer.Database.KeyExists(key);
        }

        public async void HashSet(UserCacheModel model)
        {
            await _redisServer.Database.HashSetAsync($"{CacheKeyConstants.USER_KEY}.{model.GID}", CacheUtils.ToHashEntries(model));
        }

        public async Task<string> HashGet(UserCacheModel model, string propertyName)
        {
            var value = await _redisServer.Database.HashGetAsync($"{CacheKeyConstants.USER_KEY}.{model.GID}", propertyName);
            if (value.HasValue)
            {
                return value;
            }
            throw new NotFoundException(propertyName + " cannot be found on key: " + model.GID);
        }

        public async Task<Dictionary<string, string>> HashGetAll(Guid key)
        {
            var value = await _redisServer.Database.HashGetAllAsync($"{CacheKeyConstants.USER_KEY}.{key}");
            return CacheUtils.generateMapFromHashEntry(value);
        }

        public async Task<bool> IsHashExist(Guid key)
        {
            var x  =  await _redisServer.Database.HashGetAllAsync($"{CacheKeyConstants.USER_KEY}.{key}");
            if (x != null & x.Length > 0)
                return true;
            return false;
        }

       



        //public string SortedSetGet<T>(string key, int rank)
        //{
        //    RedisValue[] values = _redisServer.Database.SortedSetRangeByRank(key, 0, 1000);
        //    RedisValue value = values[0];
        //    return RedisSerializer.Deserialize<T>(value);
        //}

    }
}
