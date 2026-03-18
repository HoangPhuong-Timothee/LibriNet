using System.Data;
using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Author = Libri.DAL.Models.Domain.Author;
using AuthorXml = Libri.DAL.Models.Xml.Author;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Libri.DAL.Models.Custom.CustomAuthor;
using Libri.DAL.Models.Custom.CustomError;
using Microsoft.EntityFrameworkCore;
using Libri.DAL.Models.Custom;

namespace Libri.BAL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly ILogger<AuthorService> _logger;
        public AuthorService(ILogger<AuthorService> logger, IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(string searchTerm)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", searchTerm, dbType: DbType.String, size: 100);

            string spName = "sp_GetAllAuthors";
            
            try
            {
                return await _uow.Dappers.ExecuteStoreProcedureReturnAsync<Author>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu tác giả: {Exception}", e);
                
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy dữ liệu tác giả.", e);
            }
        }

        public async Task<IEnumerable<AuthorWithTotalCount>> GetAllAuthorsForAdminAsync(AuthorParams authorParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", authorParams.Search, dbType: DbType.String, size: 255, direction: ParameterDirection.Input);
            parameters.Add("@PageIndex", authorParams.PageIndex, dbType: DbType.Int16, direction: ParameterDirection.Input);
            parameters.Add("@PageSize", authorParams.PageSize, dbType: DbType.Int16, direction: ParameterDirection.Input);
            
            string spName = "sp_GetAuthorsList";
            
            try
            {
               return await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<AuthorWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu tác giả: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy dữ liệu tác giả.", e);
            }
        }

        public async Task<StoreProcedureResult> AddNewAuthorAsync(Author author)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@AuthorName", author.Name, dbType: DbType.String, size: 50);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_AddNewAuthor";
            
            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm mới tác giả: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm tác giả.", e);
            }
        }

        public async Task<StoreProcedureResult> ImportAuthorsFromFileAsync(IFormFile file)
        {
            var response = new StoreProcedureResult();
            string userName = _userService.GetUserNameFromClaims();
            
            var (errors, entities) = await ValidateAndConvertToXmlImportAuthorFileAsync(file);

            if (errors.Any())
            {
                response.Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu từ file";
                response.Success = false;
                response.Errors = errors;
                return response;
            }

            string authorsXml = entities.ToXml();
            
            var parameters = new DynamicParameters();
            parameters.Add("@AuthorsXml", authorsXml, dbType: DbType.Xml, direction: ParameterDirection.Input);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255, direction: ParameterDirection.Input);
            
            string spName = "sp_BulkUpsertAuthors";
            
            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy cập nhật tác giả: {@Exception}", e);
               
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật tác giả.", e);
            }
        }

        public async Task<StoreProcedureResult> UpdateAuthorAsync(int id, Author author)
        {
            string userName = _userService.GetUserNameFromClaims();
            
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            parameters.Add("@AuthorId", id, dbType: DbType.Int32);
            parameters.Add("@AuthorName", author.Name, dbType: DbType.String, size: 50);
            
            string spName = "sp_UpdateAuthor";
            
            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu tác giả: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm tác giả.", e);
            }
        }

        public async Task<StoreProcedureResult> DeleteAuthorByIdAsync(int id)
        {
            string userName = _userService.GetUserNameFromClaims();
            
            var parameters = new DynamicParameters();
            parameters.Add("@AuthorId", id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255, direction: ParameterDirection.Input);

            string spName = "sp_SoftDeleteAuthorById";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa tác giả: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xóa tác giả.", e);
            }
        }

        public async Task<bool> CheckAuthorExistByNameAsync(string name)
        {
            try
            {
                return await _uow.Authors
                        .Queryable()
                        .Where(a => a.Name.ToLower().Trim() == name.ToLower().Trim())
                        .AnyAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi kiểm tra tác giả: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình kiểm tra tác giả");
            }
        }

        private static async Task<(List<ErrorDetails> errors, List<AuthorXml> entities)> ValidateAndConvertToXmlImportAuthorFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên tác giả"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên tác giả", "chuỗi ký tự" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên tác giả",
                    (value =>
                        !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "Tên tác giả không được để trống và chỉ chấp nhận ký tự"
                    )
                }
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên tác giả", "Name" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<AuthorXml>(file, fileValidation, headerMapping);
        }
    }
}
