using GoodJobGames.Models.CacheModel;
using GoodJobGames.Models.Responses;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoodJobGames.Utilities.Helper
{
    public static class CacheUtils
    {
        public static List<LeaderboardCacheModel> ToListLeaderboardCacheModel(RedisValue[] values)
        {
            List<LeaderboardCacheModel> response = new List<LeaderboardCacheModel>();
            foreach (var item in values)
            {
                response.Add(JsonConvert.DeserializeObject<LeaderboardCacheModel>(item.ToString()));
            }
            return response;
        }

        public static  Dictionary<string, string> generateMapFromHashEntry(HashEntry[] model)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var item in model)
            {
                keyValuePairs.Add(item.Name, item.Value);
            }
            return keyValuePairs;
        }

        public static HashEntry[] ToHashEntries(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            return properties
                .Where(x => x.GetValue(obj) != null) // <-- PREVENT NullReferenceException
                .Select
                (
                      property =>
                      {
                          object propertyValue = property.GetValue(obj);
                          string hashValue;

                          // This will detect if given property value is 
                          // enumerable, which is a good reason to serialize it
                          // as JSON!
                          if (propertyValue is IEnumerable<object>)
                          {
                              // So you use JSON.NET to serialize the property
                              // value as JSON
                              hashValue = JsonConvert.SerializeObject(propertyValue);
                          }
                          else
                          {
                              hashValue = propertyValue.ToString();
                          }

                          return new HashEntry(property.Name, hashValue);
                      }
                )
                .ToArray();
        }
    }
}
