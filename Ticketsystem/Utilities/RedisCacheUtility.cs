using StackExchange.Redis;
using Ticketsystem.Models.Data;

namespace Ticketsystem.Utilities
{
    public static class RedisCacheUtility
    {
        public static async Task DeleteCacheEntriesByPrefix(string redisServer, string redisTicketsCache)
        {
            using var redisConnection = ConnectionMultiplexer.Connect(redisServer);
            var server = redisConnection.GetServer(redisServer);

            if (server != null)
            {
                foreach (var key in server.Keys(pattern: redisTicketsCache + "*"))
                {
                    await redisConnection.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }
    }
}
