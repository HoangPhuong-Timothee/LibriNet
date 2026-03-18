using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomInventory;
using Libri.DAL.Models.Xml;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IBookInventoryService
    {
        //Inventory
        Task<IEnumerable<BookInventoryWithTotalCount>> GetAllBookInventoriesAsync(InventoryParams inventoryParams);
        Task<IEnumerable<BookInventoryTransaction>> GetBookInventoryTransactionsAsync(InventoryTransactionParams invTranParams);
        Task<ConvertedInputAndRemainingQuantity> GetConvertedInputAndRemainingQuantityAsync(ValidateBookQuantityInBookStoreParams validateParams);
        
        //Inventory receipt
        Task<IEnumerable<InventoryReceiptWithTotalCount>> GetAllInventoryReceiptsForAdminAsync(InventoryReceiptParams receiptParams);
        Task<StoreProcedureResult> ImportBookInventoriesFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> ExportBookInventoriesFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> AddImportReceiptAsync(ImportReceipt xmlModel);
        Task<StoreProcedureResult> AddExportReceiptAsync(ExportReceipt xmlModel);
        Task<StoreProcedureResult> AcceptInventoryReceiptAsync(int receiptId, string receiptType);
        Task<StoreProcedureResult> CancelInventoryReceiptAsync(int receiptId, string receiptType);

        //Inventory check
        Task<IEnumerable<InventoryAuditWithTotalCount>> GetAllInventoryAuditsAsync(InventoryAuditParams invAuditParams);
        Task<IEnumerable<AuditDetails>> GetInventoryAuditDetailsByAuditIdAsync(int auditId);
        Task<IEnumerable<AuditResult>> GetInventoryAuditResultByAuditIdAsync(int auditId);
        Task<StoreProcedureResult> AddNewInventoryAuditAsync(InventoryAudit xmlModel);
        Task<StoreProcedureResult> ConductInventoryAuditByAuditIdAsync(int auditId, List<ConductInventory> xmlModelList);
        Task<StoreProcedureResult> DeleteInventoryAuditByIdAsync(int auditId);
    }
}