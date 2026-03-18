using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Services.Interfaces;
using BookStore = Libri.DAL.Models.Domain.BookStore;
using BookStoreXml = Libri.DAL.Models.Xml.BookStore;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using Libri.DAL.Models.Custom.CustomError;
using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom.CustomBookStore;
using Microsoft.EntityFrameworkCore;
using Libri.DAL.Models.Custom;
using CloudinaryDotNet.Actions;

namespace Libri.BAL.Services
{
    public class BookStoreService : IBookStoreService
    {
        private readonly ILogger<BookStoreService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        public BookStoreService(IUnitOfWork uow, IUserService userService, ILogger<BookStoreService> logger)
        {
            _logger = logger;
            _uow = uow;
            _userService = userService;
        }

        public async Task<IEnumerable<BookStore>> GetAllBookStoresAsync()
        {
            try
            {
                return await _uow.BookStores.GetAllAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu hiệu sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi khi lấy dữ liệu hiệu sách.", e);
            }
        }

        public async Task<IEnumerable<BookStoreWithTotalCount>> GetAllBookStoresForAdminAsync(BookStoreParams bookStoreParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", bookStoreParams.Search, dbType: DbType.String, size: 255);
            parameters.Add("@PageIndex", bookStoreParams.PageIndex, dbType: DbType.Int16);
            parameters.Add("@PageSize", bookStoreParams.PageSize, dbType: DbType.Int16);

            string spName = "sp_GetBookStoresList";

            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<BookStoreWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu lấy danh sách kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy danh sách kho.", e);
            }
        }

        public async Task<StoreProcedureResult> AddNewBookStoreAsync(BookStore bookStore)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@StoreName", bookStore.StoreName, dbType: DbType.String, size: 255);
            parameters.Add("@StoreAddress", bookStore.StoreAddress, dbType: DbType.String);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_AddNewBookStore";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm hiệu sách mới: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm mới hiệu sách");
            }
        }

        public async Task<StoreProcedureResult> UpdateBookStoreAsync(int id, BookStore bookStore)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@BookStoreId", id, dbType: DbType.Int32);
            parameters.Add("@StoreName", bookStore.StoreName, dbType: DbType.String, size: 50);
            parameters.Add("@StoreAddress", bookStore.StoreAddress, dbType: DbType.String);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_UpdateBookStore";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật hiệu sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException?.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật hiệu sách.", e);
            }
        }

        public async Task<StoreProcedureResult> ImportBookStoresFromFileAsync(IFormFile file)
        {
            string userName = _userService.GetUserNameFromClaims();

            var (errors, entities) = await ValidateAndConvertToXmlBookStoreFileAsync(file);

            if (errors.Any())
            {
                return new StoreProcedureResult
                {
                    Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu",
                    Success = false,
                    Errors = errors
                };
            }

            string bookStoresXml = entities.ToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@BookStoresXml", bookStoresXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_BulkUpsertBooKStores";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm mới cửa hiệu: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi thêm mới cửa hiệu", e);
            }
        }

        public async Task<StoreProcedureResult> DeleteBookStoreByIdAsync(int id)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@BookStoreId", id, dbType: DbType.Int32);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_SoftDeleteBookStoreById";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa hiệu sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException?.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xóa hiệu sách.", e);
            }
        }

        public async Task<bool> CheckBookStoreExistByStoreNameAsync(string storeName)
        {
            try
            {
                return await _uow.BookStores
                    .Queryable()
                    .Where(bs => bs.StoreName.ToLower().Trim() == storeName.ToLower().Trim())
                    .AnyAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi kiểm tra tồn tại của hiệu sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException?.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình kiểm tra tồn tại của hiệu sách");
            }
        }

        private static async Task<(List<ErrorDetails> errors, List<BookStoreXml> entities)> ValidateAndConvertToXmlBookStoreFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên hiệu sách",
                "Địa chỉ"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên hiệu sách", "chuỗi ký tự" },
                { "Địa chỉ", "chuỗi ký tự" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên hiệu sách",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "Tên hiệu sách không được để trống"
                    )
                },
                {
                    "Địa chỉ",
                    (value => 
                        !string.IsNullOrEmpty(value),
                        "Địa chỉ hiệu sách không được bỏ trống"
                    )
                }
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên hiệu sách", "StoreName" },
                { "Địa chỉ", "StoreAddress" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<BookStoreXml>(file, fileValidation, headerMapping);
        }
    }
}
