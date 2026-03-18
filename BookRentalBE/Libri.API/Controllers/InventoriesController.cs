using AutoMapper;
using Libri.API.DTOs.Request;
using Libri.API.DTOs.Response.Errors;
using Libri.API.DTOs.Response.Inventory;
using Libri.API.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.Pagination;
using Libri.DAL.Models.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InventoriesController : ControllerBase
    {
        private readonly IBookInventoryService _inventoryService;
        private readonly IBookInventoryAuditService _inventoryAuditService;
        private readonly IBookInventoryReceiptService _inventoryReceiptService;
        private readonly IMapper _mapper;
        public InventoriesController(IMapper mapper, IBookInventoryService inventoryService, IBookInventoryAuditService inventoryAuditService, IBookInventoryReceiptService inventoryReceiptService)
        {
            _inventoryService = inventoryService;
            _inventoryAuditService = inventoryAuditService;
            _mapper = mapper;
            _inventoryReceiptService = inventoryReceiptService;
        }

        //Book inventory
        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<InventoryDTO>>> GetAllBookInventoriesAsync([FromQuery] InventoryParams inventoryParams)
        {
            var result = await _inventoryService.GetAllBookInventoriesAsync(inventoryParams);

            var bookInventories = _mapper.Map<IEnumerable<InventoryDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<InventoryDTO>(inventoryParams.PageIndex, inventoryParams.PageSize, totalItems, bookInventories);

            return Ok(response);
        }

        [HttpGet("quantity")]
        public async Task<ActionResult<ConvertedInputAndRemainingQuantityDTO>> ValidateBookQuantityInBookStoreAsync([FromQuery] ValidateBookQuantityInBookStoreParams validateParams)
        {
            var result = await _inventoryService.GetConvertedInputAndRemainingQuantityAsync(validateParams);

            var response = _mapper.Map<ConvertedInputAndRemainingQuantityDTO>(result);

            return Ok(response);
        }

        [HttpGet("inventory-transactions")]
        public async Task<ActionResult<IEnumerable<InventoryTransactionDTO>>> GetBookInventoryTransactionsAsync([FromQuery] InventoryTransactionParams invTranParams)
        {
            var result = await _inventoryService.GetBookInventoryTransactionsAsync(invTranParams);
            
            var response = _mapper.Map<IEnumerable<InventoryTransactionDTO>>(result);

            return Ok(response);
        }

        [InvalidateCache("api/Inventories")]
        [HttpPost("import/file")]
        public async Task<IActionResult> ImportBookInventoriesFromFileAsync([FromForm] IFormFile file)
        {
            var importBookInventories = await _inventoryReceiptService.ImportBookInventoriesFromFileAsync(file);

            if (!importBookInventories.Success)
            {
                return BadRequest(new APIResponse(400, importBookInventories.Message, importBookInventories.Errors));
            }

            return Ok(new { message = importBookInventories.Message });
        }

        [InvalidateCache("api/Inventories")]
        [HttpPost("import/manual")]
        public async Task<ActionResult> AddImportReceiptAsync([FromBody] AddImportInventoryReceiptRequest request) 
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState); 
            }

            var receipt = _mapper.Map<ImportReceipt>(request); 

            var addImportReceipt = await _inventoryReceiptService.AddImportReceiptAsync(receipt);
            
            if (!addImportReceipt.Success) 
            {
                return BadRequest(new APIResponse(400, addImportReceipt.Message, addImportReceipt.Errors));
            }

            return Ok(new { message = addImportReceipt.Message });    
        }

        [InvalidateCache("api/Inventories")]
        [HttpPost("export/manual")]
        public async Task<ActionResult> AddExportReceiptAsync([FromBody] AddExportInventoryReceiptRequest request)
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState); 
            }

            var receipt = _mapper.Map<ExportReceipt>(request); 

            var addExportReceipt = await _inventoryReceiptService.AddExportReceiptAsync(receipt);
            
            if (!addExportReceipt.Success) 
            {
                return BadRequest(new APIResponse(400, addExportReceipt.Message, addExportReceipt.Errors));
            }

            return Ok(new { message = addExportReceipt.Message });
        }

        [InvalidateCache("api/Inventories")]
        [HttpPost("export/file")]
        public async Task<IActionResult> ExportBookInventoriesFromFileAsync([FromForm] IFormFile file)
        {
            var exportBookInventories = await _inventoryReceiptService.ExportBookInventoriesFromFileAsync(file);

            if (!exportBookInventories.Success)
            {
                return BadRequest(new APIResponse(400, exportBookInventories.Message, exportBookInventories.Errors));
            }
            
            return Ok(new { message = exportBookInventories.Message });
        }

        //Inventory audit
        [Cached(600)]
        [HttpGet("admin/audit-list")]
        public async Task<ActionResult<Pagination<InventoryAuditDTO>>> GetAllInventoryAuditsForAdminAsync([FromQuery] InventoryAuditParams invAuditParams)
        {
            var result = await _inventoryAuditService.GetAllInventoryAuditsAsync(invAuditParams);

            var invAuditList = _mapper.Map<IEnumerable<InventoryAuditDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<InventoryAuditDTO>(invAuditParams.PageIndex, invAuditParams.PageSize, totalItems, invAuditList);

            return Ok(response);
        }

        [HttpGet("admin/audit-list/{id:int}")]
        public async Task<ActionResult<IEnumerable<InventoryAuditDetailsDTO>>> GetInventoryAuditDetailsByAuditIdAsync([FromRoute] int id)
        {
            var auditDetails = await _inventoryAuditService.GetInventoryAuditDetailsByAuditIdAsync(id);

            var response = _mapper.Map<IEnumerable<InventoryAuditDetailsDTO>>(auditDetails);

            return Ok(response);
        }

        [HttpGet("admin/audit-list/{id:int}/result")]
        public async Task<ActionResult<IEnumerable<InventoryAuditResultDTO>>> GetInventoryAuditResultByAuditIdAsync([FromRoute] int id)
        {
            var auditResult = await _inventoryAuditService.GetInventoryAuditResultByAuditIdAsync(id);

            var response = _mapper.Map<IEnumerable<InventoryAuditResultDTO>>(auditResult);

            return Ok(response);
        }

        [InvalidateCache("api/Inventories/admin/audit-list")]
        [HttpPost("admin/audit-list")]
        public async Task<IActionResult> AddNewInventoryAuditAsync([FromBody] AddInventoryAuditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || request.InventoryAuditItems.Count == 0)
            {
                return BadRequest(new APIResponse(400, "Không có dữ liệu nào được nhập."));
            }

            var inventoryAudit = _mapper.Map<InventoryAudit>(request);

            var addInventoryAudit = await _inventoryAuditService.AddNewInventoryAuditAsync(inventoryAudit);

            if (!addInventoryAudit.Success)
            {
                return BadRequest(new APIResponse(400, addInventoryAudit.Message, addInventoryAudit.Errors));
            }

            return Ok(new { message = addInventoryAudit.Message });
        }

        [InvalidateCache("api/Inventories/admin/audit-list")]
        [HttpPost("admin/audit-list/{id:int}/conduct")]
        public async Task<IActionResult> ConductInventoryAuditByAuditIdAsync([FromRoute] int id, [FromBody] List<ConductInventoryRequest> request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inventoryAuditToConduct = _mapper.Map<List<ConductInventory>>(request);

            var conductInventoryAudit = await _inventoryAuditService.ConductInventoryAuditByAuditIdAsync(id, inventoryAuditToConduct);

            if (!conductInventoryAudit.Success)
            {
                return BadRequest(new APIResponse(400, conductInventoryAudit.Message, conductInventoryAudit.Errors));
            }

            return Ok(new { message = conductInventoryAudit.Message });
        }

        [InvalidateCache("api/Inventories/admin/audit-list")]
        [HttpDelete("admin/audit-list/soft-delete/{id:int}")]
        public async Task<IActionResult> DeleteInventoryAuditByIdAsync([FromRoute] int id)
        {
            var deleteInvAudit = await _inventoryAuditService.DeleteInventoryAuditByIdAsync(id);

            if (deleteInvAudit.Success)
            {
                return BadRequest(new APIResponse(400, deleteInvAudit.Message));
            }

            return Ok(new { message = deleteInvAudit.Message });
        }

        [Cached(600)]
        [HttpGet("admin/receipt-list")]
        public async Task<ActionResult<Pagination<InventoryReceiptDTO>>> GetAllInventoryReceiptsForAdminAsync([FromQuery] InventoryReceiptParams invReceiptParams)
        {
            var result = await _inventoryReceiptService.GetAllInventoryReceiptsForAdminAsync(invReceiptParams);

            var invReceiptList = _mapper.Map<IEnumerable<InventoryReceiptDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<InventoryReceiptDTO>(invReceiptParams.PageIndex, invReceiptParams.PageSize, totalItems, invReceiptList);

            return Ok(response);
        }

        [HttpPut("admin/receipt-list/accept/{receiptId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptInventoryReceiptAsync([FromRoute] int receiptId, [FromQuery] string receiptType)
        {
            var acceptReceipt = await _inventoryReceiptService.AcceptInventoryReceiptAsync(receiptId, receiptType);

            if (!acceptReceipt.Success)
            {
                return BadRequest(new APIResponse(400, acceptReceipt.Message));
            }

            return Ok(new { message = acceptReceipt.Message });
        }

        [HttpPut("admin/receipt-list/cancel/{receiptId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CancelInventoryReceiptAsync([FromRoute] int receiptId, [FromQuery] string receiptType)
        {
            var acceptOrder = await _inventoryReceiptService.CancelInventoryReceiptAsync(receiptId, receiptType);

            if (!acceptOrder.Success)
            {
                return BadRequest(new APIResponse(400, acceptOrder.Message));
            }

            return Ok(new { message = acceptOrder.Message });
        }
    }
}