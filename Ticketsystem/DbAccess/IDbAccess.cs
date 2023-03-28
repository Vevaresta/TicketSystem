using Ticketsystem.Models.Data;

namespace Ticketsystem.DbAccess
{
    public interface IDbAccess
    {
        Task<List<T>> GetAll<T>(IFilterData data) where T : class;
        Task<T> GetById<T, TT>(TT id) where T : class;
        int GetCount(IFilterData data);
        Task Delete<T>(T entity) where T : class;
        Task Update<T>(T entity) where T : class;
        Task Add<T>(T entity) where T : class;
    }
}
