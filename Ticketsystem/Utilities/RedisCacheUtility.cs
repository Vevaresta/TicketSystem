using StackExchange.Redis;
using Ticketsystem.Models.Data;

namespace Ticketsystem.Utilities
{
    public static class RedisCacheUtility
    {
        public static async Task FlushDb(string redisServer)
        {
            using var redisConnection = ConnectionMultiplexer.Connect($"{redisServer},allowAdmin=true");
            var server = redisConnection.GetServer(redisServer);

            if (server != null)
            {
                await server.FlushDatabaseAsync();
            }
        }
    }
}
