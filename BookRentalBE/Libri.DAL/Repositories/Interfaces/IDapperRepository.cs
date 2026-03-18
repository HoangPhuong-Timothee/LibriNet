using Dapper;

namespace Libri.DAL.Repositories.Interfaces
{
    public interface IDappperRepository
    {
        Task ExecuteSqlNotReturnAsync(string query, DynamicParameters parameters = null);
        Task ExecuteStoreProcedureNotReturnAsync(string query, DynamicParameters parameters = null);
        Task<T> ExecuteSqlReturnScalarAsync<T>(string query, DynamicParameters parameters = null);
        Task<T> ExecuteStoreProcedureReturnScalarAsync<T>(string query, DynamicParameters parameters = null);
        Task<IEnumerable<T>> ExecuteSqlReturnAsync<T>(string query, DynamicParameters parameters = null);
        Task<IEnumerable<T>> ExecuteStoreProcedureReturnAsync<T>(string query, DynamicParameters parameters = null);
        Task<(T, List<U>)> ExecuteStoreProcedureReturnMultipleAsync<T, U>(string query, DynamicParameters parameters = null);
    }
}