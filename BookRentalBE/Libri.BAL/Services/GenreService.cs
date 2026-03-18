using System.Data;
using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomError;
using Libri.DAL.Models.Custom.CustomGenre;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Genre = Libri.DAL.Models.Domain.Genre;
using GenreXml = Libri.DAL.Models.Xml.Genre;

namespace Libri.BAL.Services
{
    public class GenreService : IGenreSevice
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly ILogger<GenreService> _logger;
        public GenreService(IUnitOfWork uow, IUserService userService, ILogger<GenreService> logger)
        {
            _uow = uow;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            try
            {
                return await _uow.Genres
                    .Queryable()
                    .Where(g => g.IsDeleted == false)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu thể loại: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy dữ liệu tất cả thể loại.", e);
            }
        }

        public async Task<IEnumerable<GenreWithTotalCount>> GetAllGenresForAdminAsync(GenreParams genreParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", genreParams.Search, dbType: DbType.String, size: 100);
            parameters.Add("@PageIndex", genreParams.PageIndex, dbType: DbType.Int32);
            parameters.Add("@PageSize", genreParams.PageSize, dbType: DbType.Int32);

            string spName = "sp_GetGenresList";

            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<GenreWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu thể loại: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy danh sách thể loại.");
            }
        }

        public async Task<StoreProcedureResult> AddNewGenreAsync(Genre genre)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@GenreName", genre.Name, dbType: DbType.String, size: 20);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_AddNewGenre";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            { 
                _logger.LogError("Lỗi thêm thể loại mới: {@Exception}", e);
               
                if ( e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm thể loại mới", e);
            }
        }

        public async Task<StoreProcedureResult> ImportGenresFromFileAsync(IFormFile file)
        {
            var response = new StoreProcedureResult();
            string userName = _userService.GetUserNameFromClaims();

            var (errors, entities) = await ValidateAndConvertToXmlGenreFileAsync(file);

            if (errors.Any())
            {
                response.Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu từ file";
                response.Success = false;
                response.Errors = errors;
                return response;
            }

            var genresXml = entities.ToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@GenresXml", genresXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_BulkUpsertGenres";

            try
            {
                var result = await _uow
                        .Dappers.ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm thể loại mới: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm thể loại mới", e);
            }
        }

        public async Task<StoreProcedureResult> UpdateGenreAsync(int id, Genre genre)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            parameters.Add("@GenreId", id, dbType: DbType.Int32);
            parameters.Add("@GenreName", genre.Name, dbType: DbType.String, size: 20);

            string spName = "sp_UpdateGenre";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật thể loại: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật thể loại", e);
            }
        }

        public async Task<StoreProcedureResult> DeleteGenreByIdAsync(int id)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@GenreId", id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255, direction: ParameterDirection.Input);

            string spName = "sp_SoftDeleteGenreById";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa thể loại: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xóa thể loại", e);
            }
        }

        public async Task<bool> CheckGenreExistByNameAsync(string name)
        {
            try
            {
                return await _uow.Genres
                        .Queryable()
                        .Where(g => g.Name.ToLower().Trim() == name.ToLower().Trim())
                        .AnyAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi kiểm tra thể loại: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình kiểm tra thể loại");
            }
        }

        private static async Task<(List<ErrorDetails> errors, List<GenreXml> entities)> ValidateAndConvertToXmlGenreFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên thể loại"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên thể loại", "chuỗi ký tự" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên thể loại",
                    (value =>
                        !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "Thể loại không được để trống và chỉ chấp nhận ký tự"
                    )
                }
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên thể loại", "Name" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<GenreXml>(file, fileValidation, headerMapping);
        }
    }
}
