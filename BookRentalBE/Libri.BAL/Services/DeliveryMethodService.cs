using Dapper;
using System.Globalization;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomDeliveryMethod;
using Libri.DAL.Models.Custom.CustomError;
using Libri.DAL.Models.Domain;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using DmXml = Libri.DAL.Models.Xml.DeliveryMethod;

namespace Libri.BAL.Services
{
    public class DeliveryMethodService : IDeliveryMethodService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<IDeliveryMethodService> _logger;
        private readonly IUserService _userService;
        public DeliveryMethodService(IUnitOfWork uow, ILogger<IDeliveryMethodService> looger, IUserService userService)
        {
            _logger = looger;
            _uow = uow;
            _userService = userService;
        }

        public async Task<IEnumerable<DeliveryMethodWithTotalCount>> GetAllDeliveryMethodsForAdminAsync(DeliveryMethodParams dmParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PageSize", dmParams.PageSize, dbType: DbType.Int32);
            parameters.Add("@PageIndex", dmParams.PageIndex, dbType: DbType.Int32);

            string spName = "sp_GetDeliveryMethodsList";

            try
            {
                return await _uow.Dappers.ExecuteStoreProcedureReturnAsync<DeliveryMethodWithTotalCount>(spName, parameters);
            }
            catch(Exception e)
            {
                _logger.LogError("Lỗi lấy thông tin phương thức giao sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy thông tin phương thức giao sách", e);
            }
        }

        public async Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        {
            try
            {
                return await _uow.DeliveryMethods.GetAllAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy thông tin phương thức giao sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }
            
                throw new Exception("Có lỗi xảy ra trong quá trình lấy thông tin phương thức giao sách", e);
            }
        }

        public async Task<DeliveryMethod?> GetDeliveryMethodByIdAsync(int id)
        {
            try
            {
                var dm = await _uow.DeliveryMethods.GetByIdAsync(id);
                
                if (dm == null)
                {
                    _logger.LogError("Không tìm thấy phương thức giao sách.");
                    return null;
                }

                return dm;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi  lấy thông tin phương thức giao sách theo mã {dmId}: {Exception}", e, id);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy thông tin phương thức giao sách theo mã");
            }
        }

        public async Task<StoreProcedureResult> ImportDeliveryMethodsFromFileAsync(IFormFile file)
        {
            var response = new StoreProcedureResult();
            string userName = _userService.GetUserNameFromClaims();

            var (errors, entities) = await ValidateAndConvertToXmlDmFileAsync(file);

            if (errors.Any())
            {
                response.Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu từ file";
                response.Success = false;
                response.Errors = errors;
                return response;
            }

            var dmsXml = entities.ToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@DeliveryMethodsXml", dmsXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_BulkUpsertDeliveryMethods";

            try
            {
                var result = await _uow
                        .Dappers.ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm phương thức giao hàng mới: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thêm phương thức giao hàng mới", e);
            }
        }

        public async Task<StoreProcedureResult> DeleteDeliveryMethodByIdAsync(int id)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@DeliveryMethodId", id, dbType: DbType.Int32);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_SoftDeleteDeliveryMethodById";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa phương thức giao hàng: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xóa phương thức giao hàng", e);
            }
        }

        private static async Task<(List<ErrorDetails> errors, List<DmXml> entities)> ValidateAndConvertToXmlDmFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên phương thức",
                "Thời gian giao hàng",
                "Mô tả",
                "Giá"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên phương thức", "chuỗi ký tự" },
                { "Thời gian giao hàng", "chuỗi ký tự" },
                { "Mô tả", "chuỗi ký tự" },
                { "Giá", "số nguyên" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên phương thức",
                    (value => !string.IsNullOrEmpty(value),
                        "Tên phương thức không được để trống"
                    )
                },
                {
                    "Thời gian giao hàng",
                    (value => !string.IsNullOrEmpty(value),
                        "Thời gian giao hàng không được để trống")
                },
                {
                    "Mô tả",
                    (value => string.IsNullOrEmpty(value) || value.Length <= 255,
                        "Mô tả không vượt quá 255 ký tự")
                },
                {
                    "Giá",
                    (value =>
                        {
                            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                            {
                                return price >= 0;
                            }
                            return false;
                        },
                        "Giá phải là số và lớn hơn hoặc bằng 0"
                    )
                },
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên phương thức", "ShortName" },
                { "Thời gian giao hàng", "DeliveryTime" },
                { "Mô tả", "Description" },
                { "Giá", "Price" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<DmXml>(file, fileValidation, headerMapping);
        }
    }
}
