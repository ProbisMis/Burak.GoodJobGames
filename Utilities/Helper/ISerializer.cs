using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Utilities.Helper
{
    public interface ISerializer
    {
        RedisValue Serialize<T>(T value);
        T Deserialize<T>(RedisValue value);
    }
}
