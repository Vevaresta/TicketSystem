using NuGet.Protocol.Plugins;

namespace Ticketsystem.DbAccess
{
    public interface IDbAccessFactory
    {
        public T GetDbAccess<T>() where T : class;
    }
}
