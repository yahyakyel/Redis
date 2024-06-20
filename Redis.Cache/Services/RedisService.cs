using StackExchange.Redis;

namespace Redis.Cache.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisService(string url) 
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }
        public IDatabase GetDatabase(int dbIndex)
        {
            return _connectionMultiplexer.GetDatabase(dbIndex);
        }
    }
}
