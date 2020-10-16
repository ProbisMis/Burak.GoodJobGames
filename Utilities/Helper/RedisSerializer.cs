using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Utilities.Helper
{
    public static class RedisSerializer
    {
        public static RedisValue Serialize<T>(T value)
        {
            return value.ToString();
        }
        public static string Deserialize<T>(RedisValue value)
        {
            return (string) Convert.ChangeType(value.ToString(), typeof(string));
        }

        public static List<T> Deserialize<T>(RedisValue[] value)
        {
            List<T> response = new List<T>();
            foreach (var item in value)
            {
                response.Add((T)Convert.ChangeType(value.ToString(), typeof(T)));
            }
            return response;
        }
    }
}
