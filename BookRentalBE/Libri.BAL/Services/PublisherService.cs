using System.Data;
using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.CustomError;
using Publisher = Libri.DAL.Models.Domain.Publisher;
using PublisherXml = Libri.DAL.Models.Xml.Publisher;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Libri.DAL.Models.Custom.CustomPublisher;
using Microsoft.EntityFrameworkCore;
using Libri.DAL.Models.Custom;

namespace Libri.BAL.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly ILogger<PublisherService> _logger;
        public PublisherService(IUnitOfWork uow, IUserService userService, ILogger<PublisherService> logger)
        {
            _uow = uow;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            try
            {
                return await _uow.Publishers
                    .Queryable()
                    .Where(p => p.IsDeleted == false)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu nhà xuất bản: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy dữ liệu tất cả nhà xuất bản.", e);
            }
        }

        public async Task<IEnumerable<PublisherWithTotalCount>> GetAllPublishersForAdminAsync(PublisherParams publisherParam)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", publisherParam.Search, dbType: DbType.String, size: 255, direction: ParameterDirection.Input);
            parameters.Add("@PageIndex", publisherParam.PageIndex, dbType: DbType.Int16, direction: ParameterDirection.Input);
            parameters.Add("@PageSize", publisherParam.PageSize, dbType: DbType.Int16, direction: ParameterDirection.Input);
            
            string spName = "sp_GetPublishersList";
            
            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<PublisherWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu nhà xuất bản: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu nhà xuất bản.", e);
            }
        }

        public async Task<StoreProcedureResult> AddNewPublisherAsync(Publisher publisher)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@PublisherName", publisher.Name, dbType: DbType.String, size: 100);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 100);
            parameters.Add("@Address", publisher.Address, dbType: DbType.String, size: 255);

            string spName = "sp_AddNewPublisher";
            
            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm mới nhà xuất bản: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi thêm mới nhà xuất bản.", e);
            }
        }

        public async Task<StoreProcedureResult> ImportPublishersFromFileAsync(IFormFile file)
        {
            string userName = _userService.GetUserNameFromClaims();
            
            var (errors, entities) = await ValidateAndConvertToXmlPublisherFileAsync(file);

            if (errors.Any())
            {
                return new StoreProcedureResult
                {
                    Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu",
                    Success = false,
                    Errors = errors
                };
            }

            string publishersXml = entities.ToXml();
            
            var parameters = new DynamicParameters();
            parameters.Add("@PublishersXml", publishersXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            
            string spName = "sp_BulkUpsertPublishers";
            
            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm mới nhà xuất bản: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi thêm mới nhà xuất bản", e);
            }
        }

        public async Task<StoreProcedureResult> UpdatePublisherAsync(int id, Publisher publisher)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            parameters.Add("@PublisherId", id, dbType: DbType.Int32);
            parameters.Add("@PublisherName", publisher.Name, dbType: DbType.String, size: 100);
            parameters.Add("@PublisherAddress", publisher.Address, dbType: DbType.String, size: 255);

            string spName = "sp_UpdatePublisher";
            
            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật nhà xuất bản: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi cập nhật nhà xuất bản.", e);
            }
        }

        public async Task<StoreProcedureResult> DeletePublisherByIdAsync(int id)
        {
            string userName = _userService.GetUserNameFromClaims();
            
            var parameters = new DynamicParameters();
            parameters.Add("@PublisherId", id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255, direction: ParameterDirection.Input);
            
            string spName = "sp_SoftDeletePublisherById";
            
            try 
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật nhà xuất bản: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xóa NXB.", e);
            }
        }

        public async Task<bool> CheckPublisherExistByNameAsync(string name)
        {
            try
            {
                return await _uow.Publishers
                        .Queryable()
                        .Where(p => p.Name.ToLower().Trim() == name.ToLower().Trim())
                        .AnyAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật nhà xuất bản: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình kiểm tra nxb", e);
            }
        }

        private static async Task<(List<ErrorDetails> errors, List<PublisherXml> entities)> ValidateAndConvertToXmlPublisherFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên NXB",
                "Địa chỉ"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên NXB", "chuỗi ký tự" },
                { "Địa chỉ", "chuỗi ký tự" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên NXB",
                    (value =>
                        !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "Tên nhà xuất bản không được để trống và chỉ chấp nhận ký tự"
                    )
                },
                {
                    "Địa chỉ",
                    (value => 
                        !string.IsNullOrEmpty(value),
                        "Địa chỉ của nhà xuất bản không được để trống."
                    )
                }
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên NXB", "Name" },
                { "Địa chỉ", "Address" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<PublisherXml>(file, fileValidation, headerMapping);
        }
    }
}
