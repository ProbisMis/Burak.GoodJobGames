using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Configuration;
using System.Transactions;

namespace GoodJobGames.Data
{
    public class RedisServer
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private string configurationString;
        private string configurationStringFlushDB;
        private int _currentDatabaseId = 0;

        public RedisServer(IConfiguration configuration)
        {
            CreateRedisConfigurationString(configuration);
            
            //var csredis = new CSRedis.CSRedisClient(configurationString);
            //RedisHelper.Initialization(csredis);

            _connectionMultiplexer = ConnectionMultiplexer.Connect(configurationString);
            _database = _connectionMultiplexer.GetDatabase(_currentDatabaseId);
        }

        public IDatabase Database => _database;
        public void FlushDatabase()
        {
            _connectionMultiplexer.GetServer(configurationStringFlushDB).FlushAllDatabases();
        }


        private void CreateRedisConfigurationString(IConfiguration configuration)
        {
            string host = configuration.GetSection("RedisConfiguration:Host").Value;
            string port = configuration.GetSection("RedisConfiguration:Port").Value;

            configurationString = $"{host}:{port},allowAdmin=true";
            configurationStringFlushDB = $"{host}:{port}"; 
        }

    }
}
