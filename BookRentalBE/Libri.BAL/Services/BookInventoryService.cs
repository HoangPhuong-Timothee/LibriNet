using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Enums;
using Libri.DAL.Models.Custom.CustomError;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomInventory;
using Libri.DAL.Models.DataTable;
using Libri.DAL.Models.Xml;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Globalization;

namespace Libri.BAL.Services
{
    public class BookInventoryService : IBookInventoryService
    {
        private readonly ILogger<BookInventoryService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        public BookInventoryService(IUnitOfWork uow, ILogger<BookInventoryService> logger, IUserService userService)
        {
            _logger = logger;
            _uow = uow;
            _userService = userService;
        }

        //Book inventory
        public async Task<IEnumerable<BookInventoryWithTotalCount>> GetAllBookInventoriesAsync(InventoryParams inventoryParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", inventoryParams.Search, dbType: DbType.String, size: 255);
            parameters.Add("@ISBNSearch", inventoryParams.IsbnSearch, dbType: DbType.String, size: 255);
            parameters.Add("@PageIndex", inventoryParams.PageIndex, dbType: DbType.Int16);
            parameters.Add("@PageSize", inventoryParams.PageSize, dbType: DbType.Int16);
            parameters.Add("@GenreId", inventoryParams.GenreId, dbType: DbType.Int16);
            parameters.Add("@InventoryStatus", inventoryParams.InventoryStatus, dbType: DbType.String, size: 100);
            parameters.Add("@BookStoreId", inventoryParams.BookStoreId, dbType: DbType.Int16);

            string spName = "sp_GetAllBookInventories";

            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<BookInventoryWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu tồn kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu tồn kho", e);
            }
        }

        public async Task<IEnumerable<BookInventoryTransaction>> GetBookInventoryTransactionsAsync(InventoryTransactionParams invTranParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookId", invTranParams.BookId, dbType: DbType.Int32);
            parameters.Add("@StoreName", invTranParams.StoreName, dbType: DbType.String, size: 255);
            parameters.Add("@MeasureUnitId", invTranParams.MeasureUnitId, dbType: DbType.Int32);
            parameters.Add("@TransactionType", invTranParams.TransactionType, dbType: DbType.String, size: 50);
            parameters.Add("@StartDate", invTranParams.StartDate, dbType: DbType.DateTime);
            parameters.Add("@EndDate", invTranParams.EndDate, dbType: DbType.DateTime);

            string spName = "sp_GetBookInventoryTransactionsByBookIdAndStoreName";

            try
            {
                return await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<BookInventoryTransaction>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu xuất/nhập kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu lịch sử xuất/nhập kho", e);
            }
        }

        public async Task<ConvertedInputAndRemainingQuantity> GetConvertedInputAndRemainingQuantityAsync(ValidateBookQuantityInBookStoreParams validateParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookTitle", validateParams.BookTitle, dbType: DbType.String, size: 255);
            parameters.Add("@BookStoreId", validateParams.BookStoreId, dbType: DbType.Int32);
            parameters.Add("@UnitOfMeasureId", validateParams.UnitOfMeasureId, dbType: DbType.Int32);
            parameters.Add("@InputQuantity", validateParams.InputQuantity, dbType: DbType.Int32);
            parameters.Add("@ISBN", validateParams.Isbn, dbType: DbType.String, size: 100);

            string spName = "sp_GetConvertedInputQuantityAndRemainingBookQuantity";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<ConvertedInputAndRemainingQuantity>(spName, parameters);

                return result.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu số lượng sách trong hiệu sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy dữ liệu số lượng sách trong hiệu sách", e);
            }
        }

        //Inventory receipt and hadle import - export books to inventory
        public async Task<IEnumerable<InventoryReceiptWithTotalCount>> GetAllInventoryReceiptsForAdminAsync(InventoryReceiptParams receiptParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", receiptParams.Search, dbType: DbType.String, size: 255);
            parameters.Add("@PageIndex", receiptParams.PageIndex, dbType: DbType.Int16);
            parameters.Add("@PageSize", receiptParams.PageSize, dbType: DbType.Int16);
            parameters.Add("@ReceiptStatus", receiptParams.ReceiptStatus, dbType: DbType.String, size: 100);
            parameters.Add("@ReceiptType", receiptParams.ReceiptType, dbType: DbType.String, size: 100);
            parameters.Add("@StartDate", receiptParams.StartDate, dbType: DbType.DateTime);
            parameters.Add("@EndDate", receiptParams.EndDate, dbType: DbType.DateTime);

            string spName = "sp_GetAllInventoryReceipts";

            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<InventoryReceiptWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu danh sách phiếu nhập - xuất kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu danh sách phiếu nhập - xuất kho", e);
            }
        }

        public async Task<StoreProcedureResult> ImportBookInventoriesFromFileAsync(IFormFile file)
        {
            var response = new StoreProcedureResult();
            string userName = _userService.GetUserNameFromClaims();

            var (validateErrors, entities) = await ValidateAndConvertImportInventoriesFileAsync(file);

            if (validateErrors.Any())
            {
                response.Message = "Có lỗi trong quá trình chuyển đổi dữ liệu";
                response.Success = false;
                response.Errors = validateErrors;
                return response;
            }

            var inventoriesDataTable = entities.ToDataTable();
            var rowIndex = 2;

            foreach (DataRow row in inventoriesDataTable.Rows)
            {
                row["RowNumber"] = rowIndex++;
            }

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            parameters.Add("@Inventories", inventoriesDataTable.AsTableValuedParameter("dbo.ImportInventoryTableType"));

            string spName = "sp_BulkImportBookInventories_TVPs";

            try
            {
                var result = (await _uow.Dappers.ExecuteStoreProcedureReturnAsync<dynamic>(spName, parameters)).ToList();

                if (result.Any(r => Convert.ToBoolean(r.Success) == false))
                {
                    validateErrors.AddRange(result.Where(r => Convert.ToBoolean(r.Success) == false).Select(r => new ErrorDetails
                    {
                        Location = r.Location ?? "Lỗi không xác định",
                        Details = r.Message ?? "Không xác định"
                    }));
                }

                response.Message = validateErrors.Any() ? "Có lỗi xảy ra trong quá trình tạo phiếu nhập kho" : "Tạo phiếu nhập kho thành công";
                response.Success = !validateErrors.Any();
                response.Errors = validateErrors;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi nhập kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình nhập kho", e);
            }
        }

        public async Task<StoreProcedureResult> ExportBookInventoriesFromFileAsync(IFormFile file)
        {
            var response = new StoreProcedureResult();
            string userName = _userService.GetUserNameFromClaims();

            var (validateErrors, entities) = await ValidateAndConvertExportInventoriesFileAsync(file);

            if (validateErrors.Any())
            {
                response.Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu từ file";
                response.Success = false;
                response.Errors = validateErrors;
                return response;
            }

            var inventoriesDataTable = entities.ToDataTable();
            var rowIndex = 2;

            foreach (DataRow row in inventoriesDataTable.Rows)
            {
                row["RowNumber"] = rowIndex++;
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Inventories", inventoriesDataTable.AsTableValuedParameter("dbo.ExportInventoryTableType"));
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_BulkExportBookInventories_TVPs";

            try
            {
                var result = (await _uow.Dappers.ExecuteStoreProcedureReturnAsync<dynamic>(spName, parameters)).ToList();

                if (result.Any(r => Convert.ToBoolean(r.Success) == false))
                {
                    validateErrors.AddRange(result.Where(r => Convert.ToBoolean(r.Success) == false).Select(r => new ErrorDetails
                    {
                        Location = r.Location ?? "Lỗi hệ thống",
                        Details = r.Message ?? "Không xác định"
                    }));
                }

                response.Message = validateErrors.Any() ? "Có lỗi xảy ra trong quá trình tạo phiếu xuất kho" : "Tạo phiếu xuất kho thành công";
                response.Success = !validateErrors.Any();
                response.Errors = validateErrors;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xuất kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xuất kho", e);
            }
        }

        public async Task<StoreProcedureResult> AddImportReceiptAsync(ImportReceipt xmlModel)
        {
            var userName = _userService.GetUserNameFromClaims();

            var importReceiptXml = xmlModel.SingleObjectToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            parameters.Add("@ImportReceiptXml", importReceiptXml, dbType: DbType.Xml);

            string spName = "sp_AddNewImportReceipt";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi nhập kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình nhập kho", e);
            }
        }

        public async Task<StoreProcedureResult> AddExportReceiptAsync(ExportReceipt xmlModel)
        {
            var userName = _userService.GetUserNameFromClaims();

            var exportReceiptXml = xmlModel.SingleObjectToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            parameters.Add("@ExportReceiptXml", exportReceiptXml, dbType: DbType.Xml);

            string spName = "sp_AddNewExportReceipt";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xuất kho: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xuất kho", e);
            }
        }

        public async Task<StoreProcedureResult> AcceptInventoryReceiptAsync(int receiptId, string receiptType)
        {
            var receiptStatus = InventoryReceiptStatus.Accept.ToString();

            var parameters = new DynamicParameters();
            parameters.Add("@ReceiptId", receiptId, dbType: DbType.Int32);
            parameters.Add("@ReceiptType", receiptType, dbType: DbType.String, size: 100);
            parameters.Add("@ReceiptStatus", receiptStatus, dbType: DbType.String, size: 100);

            string spName = "sp_AcceptInventoryReceiptByReceiptId";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật trạng thái mã phiếu '#{receiptId}': {Exception}", receiptId, e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật trạng thái phiếu kho", e);
            }
        }

        public async Task<StoreProcedureResult> CancelInventoryReceiptAsync(int receiptId, string receiptType)
        {
            var receiptStatus = InventoryReceiptStatus.Cancel.ToString();

            var parameters = new DynamicParameters();
            parameters.Add("@ReceiptId", receiptId, dbType: DbType.Int32);
            parameters.Add("@ReceiptType", receiptType, dbType: DbType.String, size: 100);
            parameters.Add("@ReceiptStatus", receiptStatus, dbType: DbType.String, size: 100);

            string spName = "sp_CancelInventoryReceiptByReceiptId";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật trạng thái mã phiếu '#{receiptId}': {Exception}", receiptId, e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật trạng thái phiếu kho", e);
            }
        }

        //Count and check inventory quantiy
        public async Task<IEnumerable<InventoryAuditWithTotalCount>> GetAllInventoryAuditsAsync(InventoryAuditParams invAuditParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", invAuditParams.Search, dbType: DbType.String, size: 255);
            parameters.Add("@PageIndex", invAuditParams.PageIndex, dbType: DbType.Int16);
            parameters.Add("@PageSize", invAuditParams.PageSize, dbType: DbType.Int16);
            parameters.Add("@AuditStatus", invAuditParams.AuditStatus, dbType: DbType.String, size: 100);
            parameters.Add("@StartDate", invAuditParams.StartDate, dbType: DbType.DateTime);
            parameters.Add("@EndDate", invAuditParams.EndDate, dbType: DbType.DateTime);
            parameters.Add("@AudittedBy", invAuditParams.AudittedBy, dbType: DbType.String, size: 255);

            string spName = "sp_GetAllInventoryAudits";

            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<InventoryAuditWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu danh sách kế hoạch kiểm kê hàng hóa: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu danh sách kế hoạch kiểm kê hàng hóa", e);
            }
        }

        public async Task<IEnumerable<AuditDetails>> GetInventoryAuditDetailsByAuditIdAsync(int auditId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AuditId", auditId, dbType: DbType.Int32);

            string spName = "sp_GetInventoryAuditDetailsByAuditId";

            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<AuditDetails>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu danh sách hàng hóa kiểm kê: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu danh sách hàng hóa kiểm kê", e);
            }
        }

        public async Task<IEnumerable<AuditResult>> GetInventoryAuditResultByAuditIdAsync(int auditId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AuditId", auditId, dbType: DbType.Int32);

            string spName = "sp_GetInventoryAuditResultByAuditId";

            try
            {
                return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<AuditResult>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu kết quả kiểm kê: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ kết quả kiểm kê", e);
            }
        }

        public async Task<StoreProcedureResult> AddNewInventoryAuditAsync(InventoryAudit xmlModel)
        {
            var userName = _userService.GetUserNameFromClaims();

            var inventoryAuditXml = xmlModel.SingleObjectToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);
            parameters.Add("@InventoryAuditXml", inventoryAuditXml, dbType: DbType.Xml);

            string spName = "sp_AddNewInventoryAudit";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi tạo kế hoạch kiểm kê: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình tạo kế hoạch kiểm kê", e);
            }
        }

        public async Task<StoreProcedureResult> ConductInventoryAuditByAuditIdAsync(int auditId, List<ConductInventory> xmlModelList)
        {
            var userName = _userService.GetUserNameFromClaims();

            var conductInventoryXml = xmlModelList.ToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@AuditId", auditId, dbType: DbType.Int32);
            parameters.Add("@ConductInventoryXml", conductInventoryXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, DbType.String, size: 255);

            string spName = "sp_ConductInventoryByAuditId";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thực hiện kiểm kê theo kế hoạch mã {AuditId}: {Exception}", auditId, e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình thực hiện kiểm kê theo kế hoạch", e);
            }
        }

        public async Task<StoreProcedureResult> DeleteInventoryAuditByIdAsync(int id)
        {
            var userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@InvAuditId", id, dbType: DbType.Int32);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_SoftDeleteInventoryAuditById";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa kế hoạch kiểm kê: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình xóa kế hoạch kiểm kê", e);
            }
        }

        //Validate file upload function
        private static async Task<(List<ErrorDetails> valdiateErrors, List<ImportInventoryDT> entities)> ValidateAndConvertImportInventoriesFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên sách",
                "Đơn vị",
                "Số lượng",
                "Hiệu sách",
                "Ngày nhập",
                "Ghi chú",
                "ISBN"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên sách", "chuỗi ký tự" },
                { "Hiệu sách", "chuỗi ký tự" },
                { "Đơn vị", "chuỗi ký tự" },
                { "Số lượng", "số nguyên" },
                { "Ngày nhập", "ngày tháng năm" },
                { "Ghi chú", "chuỗi ký tự" },
                { "ISBN", "chuỗi ký tự" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên sách",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "Tên sách không được để trống"
                    )
                },
                {
                    "ISBN",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "ISBN không được để trống"
                    )
                },
                {
                    "Hiệu sách",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "Hiệu sách không được để trống"
                    )
                },
                {
                    "Số lượng",
                    (value =>
                        int.TryParse(value, out var quantity) && quantity >= 0,
                        "Số lượng nhập phải là số và lớn hơn hoặc bằng 0"
                    )
                },
                {
                    "Đơn vị",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "Đơn vị không được để trống"
                    )
                },
               {
                    "Ngày nhập",
                    (value =>
                        DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) && date <= DateTime.Now,
                        "Ngày nhập kho phải có định dạng dd/MM/yyyy, không được để trống và không được ở ngày tương lai"
                )
                },
                {
                    "Ghi chú",
                    (value =>
                        string.IsNullOrEmpty(value) || value.Length <= 100,
                        "Ghi chú không vượt quá 100 ký tự"
                    )
                }
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên sách", "BookTitle" },
                { "Số lượng", "Quantity" },
                { "Đơn vị", "MeasureUnit" },
                { "Hiệu sách", "StoreName" },
                { "Ngày nhập", "ImportDate" },
                { "Ghi chú", "ImportNotes" },
                { "ISBN", "ISBN" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<ImportInventoryDT>(file, fileValidation, headerMapping);
        }

        private static async Task<(List<ErrorDetails> validateErrors, List<ExportInventoryDT> entities)> ValidateAndConvertExportInventoriesFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tên sách",
                "Hiệu sách",
                "Số lượng",
                "Đơn vị",
                "Ngày xuất",
                "Ghi chú",
                "ISBN"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tên sách", "chuỗi ký tự" },
                { "Hiệu sách", "chuỗi ký tự" },
                { "Đơn vị", "chuỗi ký tự" },
                { "Số lượng", "số nguyên" },
                { "Ngày xuất", "ngày tháng năm" },
                { "Ghi chú", "chuỗi ký tự" },
                { "ISBN", "chuỗi ký tự" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tên sách",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "Tên sách không được để trống"
                    )
                },
                 {
                    "ISBN",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "ISBN không được để trống"
                    )
                },
                {
                    "Hiệu sách",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "Hiệu sách không được để trống"
                    )
                },
                {
                    "Số lượng",
                    (value =>
                        int.TryParse(value, out var quantity) && quantity >= 0,
                        "Số lượng xuất kho phải là số và lớn hơn hoặc bằng 0"
                     )
                },
                {
                    "Đơn vị",
                    (value =>
                        !string.IsNullOrEmpty(value),
                        "Đơn vị không được để trống"
                    )
                },
                {
                    "Ngày xuất",
                    (value =>
                        DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) && date <= DateTime.Now,
                        "Ngày nhập kho phải có định dạng dd/MM/yyyy, không được để trống và không được ở ngày tương lai"
                )
                },
                {
                    "Ghi chú",
                    (value =>
                        !string.IsNullOrEmpty(value) && value.Length <= 100,
                        "Ghi chú không được để trống và không vượt quá 100 ký tự"
                     )
                }
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tên sách", "BookTitle" },
                { "Hiệu sách", "StoreName" },
                { "Đơn vị", "MeasureUnit" },
                { "Số lượng", "Quantity" },
                { "Ngày xuất", "ExportDate" },
                { "Ghi chú", "ExportNotes" },
                { "ISBN", "ISBN" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<ExportInventoryDT>(file, fileValidation, headerMapping);
        }
    }
}