using System.Data;
using Dapper;
using Libri.DAL.DatabaseContext;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class DapperRepository : IDappperRepository
    {
        private readonly CustomContext _customContext;
        public DapperRepository(CustomContext customContext)
        {
            _customContext = customContext;
        }

        public async Task ExecuteSqlNotReturnAsync(string query, DynamicParameters parameters = null)
        {
            using var con = _customContext.CreateConnection();
            await con.ExecuteAsync(query, parameters, commandType: CommandType.Text);
        }

        public async Task ExecuteStoreProcedureNotReturnAsync(string query, DynamicParameters parameters = null)
        {
            using var con = _customContext.CreateConnection();
            await con.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> ExecuteSqlReturnScalarAsync<T>(string query, DynamicParameters parameters = null)
        {
            using var con = _customContext.CreateConnection();
            return await con.ExecuteScalarAsync<T>(query, parameters, commandType: CommandType.Text);
        }

        public async Task<T> ExecuteStoreProcedureReturnScalarAsync<T>(string query, DynamicParameters parameters = null)
        {
            using var con = _customContext.CreateConnection();
            return await con.ExecuteScalarAsync<T>(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> ExecuteSqlReturnAsync<T>(string query, DynamicParameters parameters = null)
        {
            using var con = _customContext.CreateConnection();
            return await con.QueryAsync<T>(query, parameters, commandType: CommandType.Text, commandTimeout: 180);
        }

        public async Task<IEnumerable<T>> ExecuteStoreProcedureReturnAsync<T>(string query, DynamicParameters parameters = null)
        {
            using var con = _customContext.CreateConnection();
            return await con.QueryAsync<T>(query, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 180);
        }

        public async Task<(T, List<U>)> ExecuteStoreProcedureReturnMultipleAsync<T, U>(string query, DynamicParameters parameters = null)
        {
            using (var con = _customContext.CreateConnection())
            {
                con.Open();

                using (var multi = await con.QueryMultipleAsync(query, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 180))
                {
                    var result1 = (await multi.ReadAsync<T>()).FirstOrDefault();
                    var result2 = (await multi.ReadAsync<U>()).ToList();
                    return (result1, result2);
                }
            }
        }
    }
}