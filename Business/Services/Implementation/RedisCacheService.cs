using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data;
using GoodJobGames.Models;
using GoodJobGames.Models.CacheModel;
using GoodJobGames.Models.Responses;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public bool SortedSetAny<T>(string key, T data)
        {
            RedisValue value = RedisSerializer.Serialize(data);
            var x = _redisServer.Database.SortedSetScore(key, value);
            return  x == null ? false : true;
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

        public async Task<int> SortedSetGetRank(string key, LeaderboardCacheModel data)
        {
            var x = await _redisServer.Database.SortedSetRankAsync(key, JsonConvert.SerializeObject(data), Order.Descending);
            if (x.HasValue)
            {
                return Convert.ToInt32(x.Value);
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

        public void SortedSetAddRange<T>(string key, List<T> data)
        {
            throw new NotImplementedException();
        }


        public List<LeaderboardCacheModel> SortedSetGetAll(string key)
        {
            RedisValue[] values = _redisServer.Database.SortedSetRangeByRank(key);
            return CacheUtils.ToListLeaderboardCacheModel(values);
        }

        public bool hasAny(string key)
        {
            return _redisServer.Database.KeyExists(key);
        }

        public Task<string> SortedSetGetAllCacheModel(string key)
        {
            throw new NotImplementedException();
        }

        public async void HashSet(UserCacheModel model)
        {
            await _redisServer.Database.HashSetAsync($"{CacheKeyConstants.USER_KEY}.{model.GID}", CacheUtils.ToHashEntries(model));
        }

        public async Task<string> HashGet(UserCacheModel model, string propertyName)
        {
            var value = await _redisServer.Database.HashGetAsync($"{CacheKeyConstants.USER_KEY}.{model.GID}", propertyName);
            return value;
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
