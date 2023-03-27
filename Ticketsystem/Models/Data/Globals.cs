namespace Ticketsystem.Models.Data
{
    public class Globals
    {
        public bool EnableRedisCache { get; set; } = false;
        public string RedisServer { get; set; }
        public string RedisTicketsCache { get; set; }
    }
}
