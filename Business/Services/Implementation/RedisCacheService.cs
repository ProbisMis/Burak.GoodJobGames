using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data;
using GoodJobGames.Models;
using GoodJobGames.Models.Responses;
using GoodJobGames.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void SortedSetAdd(string key, int score, UserResponse data)
        {
            _redisServer.Database.SortedSetAdd(key, JsonConvert.SerializeObject(data), score);
        }

        public void SortedSetIncrement(string key, int score, UserResponse data)
        {
            _redisServer.Database.SortedSetRemove(key, JsonConvert.SerializeObject(data));
            data.Score += score;
            SortedSetAdd(key, score, data);
        }

        public int SortedSetGetRank(string key, UserResponse data)
        {
            var x =  _redisServer.Database.SortedSetRank(key, JsonConvert.SerializeObject(data));
            if (x.HasValue)
            {
                return Convert.ToInt32(x.Value);
            }
            return -1;
        }

        public int SortedSetGetScore(string key, UserResponse data)
        {
            var x = _redisServer.Database.SortedSetScore(key, JsonConvert.SerializeObject(data));
            if (x.HasValue)
            {
                return Convert.ToInt32(x.Value);
            }
            return -1;
        }

        public string SortedSetIntersect(string firstKey, string secondKey, string destination)
        {
            var x = _redisServer.Database.SortedSetCombineAndStore(SetOperation.Intersect, destination, firstKey, secondKey);
            return x.ToString();
        }

        public void SortedSetAddRange<T>(string key, List<T> data)
        {
            throw new NotImplementedException();
        }


        public List<T> SortedSetGetAll<T>(string key)
        {
            RedisValue[] values = _redisServer.Database.SortedSetRangeByRank(key);
            return RedisSerializer.Deserialize<T>(values);
        }

        public bool hasAny(string key)
        {
            return _redisServer.Database.KeyExists(key);
        }

        public string SortedSetGetAllCacheModel(string key)
        {
            throw new NotImplementedException();
        }

        //public string SortedSetGet<T>(string key, int rank)
        //{
        //    RedisValue[] values = _redisServer.Database.SortedSetRangeByRank(key, 0, 1000);
        //    RedisValue value = values[0];
        //    return RedisSerializer.Deserialize<T>(value);
        //}

    }
}
