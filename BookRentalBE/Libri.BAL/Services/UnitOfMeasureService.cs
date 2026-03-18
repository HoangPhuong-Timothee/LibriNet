using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomError;
using Libri.DAL.Models.Custom.CustomUnitOfMeasure;
using Libri.DAL.Models.Domain;
using Libri.DAL.Models.Xml;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Globalization;

namespace Libri.BAL.Services
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UnitOfMeasureService> _logger;
        private readonly IUserService _userService;
        public UnitOfMeasureService(IUserService userService, IUnitOfWork uow, ILogger<UnitOfMeasureService> logger)
        {
            _uow = uow;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IEnumerable<UnitOfMeasureWithTotalCount>> GetAllUnitOfMeasuresForAdminAsync(UnitOfMeasureParams uomParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PageIndex", uomParams.PageIndex, dbType: DbType.Int32);
            parameters.Add("@PageSize", uomParams.PageSize, dbType: DbType.Int32);
            parameters.Add("@Search", uomParams.Search, dbType: DbType.String, size: 100);

            string spName = "sp_GetUnitOfMeasuresList";

            try
            {
                return await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<UnitOfMeasureWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu đơn vị tính: {Exception}", e);

                if (e.InnerException != null) 
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu đơn vị tính.", e);
            }
        }

        public async Task<IEnumerable<UnitOfMeasure>> GetAllUnitOfMeasuresAsync()
        {
            try
            {
                return await _uow.UnitOfMeasures
                        .Queryable()
                        .Where(uom => uom.IsDeleted == false)
                        .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu đơn vị tính: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu đơn vị tính");
            }
        }

        public async Task<StoreProcedureResult> AddNewUnitOfMeasureAsync(MeasureUnit uomXmlModel)
        {
            string userName = _userService.GetUserNameFromClaims();

            string measureUnitXml = uomXmlModel.SingleObjectToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@MeasureUnitXml", measureUnitXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_AddNewUnitOfMeasure";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm đơn vị đo mới: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm đơn vị đo mới.", e);
            }
        }

        public async Task<StoreProcedureResult> ImportUnitOfMeasuresFromFileAsync(IFormFile file)
        {
            string userName = _userService.GetUserNameFromClaims();

            var (errors, entities) = await ValidateAndConvertToXmlMeasureUnitFileAsync(file);

            if (errors.Any())
            {
                return new StoreProcedureResult
                {
                    Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu từ file",
                    Success = false,
                    Errors = errors
                };
            }

            var measureUnitsXml = entities.ToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@MeasureUnitsXml", measureUnitsXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_BulkUpsertMeasureUnits";

            try
            {
                var result = await _uow
                        .Dappers.ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm đơn vị đo mới từ file: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm đơn vị mới từ file.", e);
            }
        }

        public async Task<StoreProcedureResult> UpdateUnitOfMeasureAsync(int id, MeasureUnit uomXmlModel)
        {
            string userName = _userService.GetUserNameFromClaims();

            string measureUnitXml = uomXmlModel.SingleObjectToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@MeasureUnitId", id, dbType: DbType.Int32);
            parameters.Add("@MeasureUnitXml", measureUnitXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_UpdateUnitOfMeasure";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật đơn vị đo mới: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật đơn vị đo mới.", e);
            }
        }

        public async Task<StoreProcedureResult> DeleteUnitOfMeasureByIdAsync(int id)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@UnitOfMeasureId", id, dbType: DbType.Int32);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_SoftDeleteUnitOfMeasureById";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa đơn vị đo: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xóa đơn vị đo.", e);
            }
        }

        public async Task<bool> CheckUnitOfMeasureExistByNameAsync(string name)
        {
            try
            {
                return await _uow.UnitOfMeasures
                        .Queryable()
                        .Where(uom => uom.Name.ToLower().Trim() == name.ToLower().Trim())
                        .AnyAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi kiểm tra đơn vị tồn tại: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình kiểm tra tồn tại của đơn vị tính.", e);
            }
        }

        private static async Task<(List<ErrorDetails> errors, List<ImportMeasureUnit> entities)> ValidateAndConvertToXmlMeasureUnitFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên đơn vị",
                "Mô tả",
                "Đơn vị gốc quy đổi",
                "Tỷ lệ quy đổi"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên đơn vị", "chuỗi ký tự" },
                { "Mô tả", "chuỗi ký tự" },
                { "Đơn vị gốc quy đổi", "chuỗi ký tự" },
                { "Tỷ lệ quy đổi", "số thập phân" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên đơn vị",
                    (value =>
                        !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "Đơn vị không được để trống và chỉ chấp nhận ký tự"
                    )
                },
                {
                    "Mô tả",
                   (value =>
                        string.IsNullOrEmpty(value) || value.Length <= 255,
                        "Mô tả không vượt quá 255 ký tự"
                    )
                },
                {
                    "Đơn vị gốc quy đổi",
                    (value =>
                        !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "Đơn vị gốc để quy đổi không được để trống và chỉ chấp nhận ký tự"
                    )
                },
                {
                    "Tỷ lệ quy đổi",
                    (value =>
                        {
                            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var conversionRate)) 
                            {
                                return conversionRate >= 0;
                            }
                            return false;
                        },
                        "Tỷ lệ quy đổi phải là 1 số và không được âm"
                    )
                },
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên đơn vị", "SrcUnitName" },
                { "Mô tả", "Description" },
                { "Đơn vị gốc quy đổi", "DestUnitName" },
                { "Tỷ lệ quy đổi", "ConversionRate" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<ImportMeasureUnit>(file, fileValidation, headerMapping);
        }
    }
}
