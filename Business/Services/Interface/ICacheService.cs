using GoodJobGames.Models;
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
        List<T> SortedSetGetAll<T>(string key);
        void Remove(string key);
        void Clear();
        bool hasAny(string key);
        int SortedSetGetScore(string key, UserResponse data);
        int SortedSetGetRank(string key, UserResponse data);
        void SortedSetAdd(string key, int score, UserResponse data);
        void SortedSetIncrement(string key, int score, UserResponse data);
        string SortedSetGetAllCacheModel(string key);
        string SortedSetIntersect(string firstKey, string secondKey, string destination);
    }
}
