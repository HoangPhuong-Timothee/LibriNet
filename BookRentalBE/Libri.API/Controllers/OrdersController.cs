using AutoMapper;
using Libri.API.DTOs.Request;
using Libri.API.DTOs.Response.Errors;
using Libri.API.DTOs.Response.Order;
using Libri.API.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.Pagination;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IDeliveryMethodService _dmService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IDeliveryMethodService dmService, IMapper mapper)
        {
            _orderService = orderService;
            _dmService = dmService;
            _mapper = mapper;
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<OrderDTO>> CreateOrderAsync([FromRoute] string basketId, [FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = _mapper.Map<Order>(request);

            var createOrder = await _orderService.CreateOrderAsync(basketId, order);

            if (!createOrder.Success)
            {
                return BadRequest(new APIResponse(400, createOrder.Message));
            }
            
            return StatusCode(201, new { message = createOrder.Message });
        }

        [HttpGet("admin/orders-list")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Pagination<OrderDTO>>> GetAllOrdersForAdminAsync([FromQuery] OrderParams orderParams)
        {
            var result = await _orderService.GetAllOrdersForAdminAsync(orderParams);
            
            var orders = _mapper.Map<IEnumerable<OrderDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;
            
            var response = new Pagination<OrderDTO>(orderParams.PageIndex, orderParams.PageSize, totalItems, orders);
            
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<OrderDTO>>> GetOrdersByUserEmailAsync([FromQuery] OrderParams orderParams)
        {
            var result = await _orderService.GetOrdersByUserEmailAsync(orderParams);
            
            var orders = _mapper.Map<IEnumerable<OrderDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;
            
            var response = new Pagination<OrderDTO>(orderParams.PageIndex, orderParams.PageSize, totalItems, orders);
            
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderWithDetailsDTO>> GetOrderByOrderIdAndUserEmailAsync([FromRoute] int id)
        {
            var orderWithDetails = await _orderService.GetOrderByOrderIdAndUserEmailAsync(id);

            var response = _mapper.Map<OrderWithDetailsDTO>(orderWithDetails);
            
            return Ok(response);
        }

        [Cached(600)]
        [HttpGet("admin/delivery-methods-list")]
        public async Task<ActionResult<Pagination<DeliveryMethodDTO>>> GetAllDeliveryMethodsForAdminAsync([FromQuery] DeliveryMethodParams dmParams)
        {
            var result = await _dmService.GetAllDeliveryMethodsForAdminAsync(dmParams);

            var deliveryMethods = _mapper.Map<IEnumerable<DeliveryMethodDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<DeliveryMethodDTO>(dmParams.PageIndex, dmParams.PageSize, totalItems, deliveryMethods);

            return Ok(response);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await _dmService.GetAllDeliveryMethodsAsync();

            var response = _mapper.Map<IEnumerable<DeliveryMethodDTO>>(deliveryMethods);

            return Ok(response);
        }

        [InvalidateCache("api/Orders/delivery-method")]
        [HttpPost("delivery-method/import-from-file")]
        public async Task<IActionResult> ImportDeliveryMethodsFromFileAsync([FromForm] IFormFile file)
        {
            var importDms = await _dmService.ImportDeliveryMethodsFromFileAsync(file);

            if (!importDms.Success)
            {
                return BadRequest(new APIResponse(400, importDms.Message, importDms.Errors));
            }

            return StatusCode(201, new { message = importDms.Message });
        }

        [InvalidateCache("api/Orders/delivery-method")]
        [HttpDelete("delivery-method/soft-delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteGenreAsync([FromRoute] int id)
        {
            var deleteGenre = await _dmService.DeleteDeliveryMethodByIdAsync(id);

            if (!deleteGenre.Success)
            {
                return BadRequest(new APIResponse(400, deleteGenre.Message));
            }

            return Ok(new { message = deleteGenre.Message });
        }

        [HttpPut("accept/{userEmail}/{orderId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AcceptOrderAsync([FromRoute] string userEmail, [FromRoute] int orderId)
        {
            var acceptOrder = await _orderService.AcceptOrderAsync(orderId, userEmail);

            if (!acceptOrder.Success)
            {
                return BadRequest(new APIResponse(400, acceptOrder.Message));
            }

            return Ok(new { message = acceptOrder.Message });
        }

        [HttpPut("decline/{userEmail}/{orderId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeclineOrderAsync([FromRoute] string userEmail, [FromRoute] int orderId)
        {
            var acceptOrder = await _orderService.DeclineOrderAsync(orderId, userEmail);

            if (!acceptOrder.Success)
            {
                return BadRequest(new APIResponse(400, acceptOrder.Message));
            }

            return Ok(new { message = acceptOrder.Message });
        }
    }
}
