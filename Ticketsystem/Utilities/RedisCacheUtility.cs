using StackExchange.Redis;
using Ticketsystem.Models.Data;

namespace Ticketsystem.Utilities
{
    public static class RedisCacheUtility
    {
        public static async Task DeleteCacheEntriesByPrefix(Globals globals, string prefix)
        {
            using var redisConnection = ConnectionMultiplexer.Connect(globals.RedisServer);
            var server = redisConnection.GetServer(globals.RedisServer);

            if (server != null)
            {
                foreach (var key in server.Keys(pattern: globals.RedisTicketsCache + "*" + prefix + "*"))
                {
                    await redisConnection.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }
    }
}
