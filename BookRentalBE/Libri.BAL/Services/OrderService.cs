using System.Data;
using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.CustomOrder;
using Libri.DAL.UnitOfWork;
using Microsoft.Extensions.Logging;
using OrderXml = Libri.DAL.Models.Xml.Order;
using OrderItemXml = Libri.DAL.Models.Xml.OrderItem;
using Order = Libri.DAL.Models.Domain.Order;
using Libri.DAL.Models.Custom;
using Libri.DAL.Enums;

namespace Libri.BAL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUserService _userService;
        private readonly IBasketService _basketService;
        private readonly IDeliveryMethodService _dmService;
        private readonly IBookService _bookService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IUnitOfWork uow, IUserService userService, IDeliveryMethodService dmService, IBasketService basketService, IBookService bookService, ILogger<OrderService> logger)
        {
            _uow = uow;
            _userService = userService;
            _dmService = dmService;
            _basketService = basketService;
            _bookService = bookService;
            _logger = logger;
        }

        public async Task<StoreProcedureResult> CreateOrderAsync(string basketId, Order order)
        {
            var response = new StoreProcedureResult();
            string userEmail = _userService.GetUserEmailFromClaims();
            
            //Get the basket to make order
            var basket = await _basketService.GetBasketByKeyAsync(basketId);

            if (basket == null)
            {
                _logger.LogError("Giỏ hàng {basketId} không tìm thấy", basketId);
                response.Success = false;
                response.Message = "Không tìm thấy giỏ hàng";
                return response;
            }
            
            //Get books in the basket and map it to the order item of order
            var orderItems = new List<OrderItemXml>();

            foreach (var basketItem in basket!.BasketItems)
            {
                var book = await _bookService.GetBookByIdAsync(basketItem.Id);

                if (book == null)
                {
                    _logger.LogError("Có lỗi xảy ra trong quá trình tạo đơn đặt sách, không tìm thấy sách {bookTitle} trong giỏ", basketItem.BookTitle);
                    response.Success = false;
                    response.Message = "Có lỗi xảy ra trong việc lấy thông tin sách trong giỏ";
                    return response;
                }

                var orderItem = new OrderItemXml
                {
                    BookId = basketItem.Id,
                    BookTitle = basketItem.BookTitle,
                    BookImageUrl = basketItem.ImageUrl,
                    Price = basketItem.Price,
                    Quantity = basketItem.Quantity
                };
                orderItems.Add(orderItem);
            }

            //Get details of delivery method selected
            var deliveryMethod = await _dmService.GetDeliveryMethodByIdAsync(order.DeliveryMethodId);

            if (deliveryMethod == null)
            {
                _logger.LogError("Dịch vụ giao hàng của giỏ sách chưa được chọn hoặc không tim thấy");
                response.Message = "Dịch vụ giao hàng của giỏ sách chưa được chọn hoặc không tìm thấy";
                response.Success = false;
                return response;
            }

            var shippingAddress = new ShippingAddress
            {
                FullName = order.FullName!,
                Street = order.Street!,
                City = order.City!,
                PostalCode = order.PostalCode!,
                Ward = order.Ward!,
                District = order.District!
            };
            
            var subtotal = orderItems.Sum(x => x.Price * x.Quantity);

            var orderXmlModel = new OrderXml
            {
                UserEmail = userEmail,
                OrderItems = orderItems,
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = shippingAddress,
                Subtotal = subtotal,
                PaymentIntentId = basket.Id
            };

            string createOrderXml = orderXmlModel.SingleObjectToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderXml", createOrderXml, dbType: DbType.Xml);
            
            string spName = "sp_CreateOrder";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi tạo đơn hàng: {Exception}", e);
               
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình tạo đơn hàng.", e);
            }
        }

        public async Task<OrderWithDetails> GetOrderByOrderIdAndUserEmailAsync(int orderId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);

            string spName = "sp_GetOrderDetailsByOrderId";

            try
            {
                var (orderDetails, orderItems) = await _uow.Dappers
                        .ExecuteStoreProcedureReturnMultipleAsync<OrderWithDetails, OrderItem>(spName, parameters);

                if (orderDetails != null)
                {
                    orderDetails.OrderItems = orderItems.ToList();
                }

                return orderDetails!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy thông tin chi tiết đơn hàng mã '#{orderId}': {Exception}", orderId, e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Lỗi lấy thông tin chi tiết đơn hàng", e);
            }
        }

        public async Task<IEnumerable<OrderWithTotalCount>> GetAllOrdersForAdminAsync(OrderParams orderParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PageIndex", orderParams.PageIndex, dbType: DbType.Int32);
            parameters.Add("@PageSize", orderParams.PageSize, dbType: DbType.Int32);
            parameters.Add("@OrderStatus", orderParams.OrderStatus, dbType: DbType.String, size: 100);
            parameters.Add("@StartDate", orderParams.StartDate, dbType: DbType.DateTime);
            parameters.Add("@EndDate", orderParams.EndDate, dbType: DbType.DateTime);
            parameters.Add("@OrderEmail", orderParams.OrderEmail, dbType: DbType.String, size: 255);

            string spName = "sp_GetAllUserOrders";

            try
            {
                return await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<OrderWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy thông tin tất cả đơn đặt hàng: {Exception}", e);

               if (e.InnerException != null)
               {
                   _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
               }

               throw new Exception("Có lỗi xảy ra trong quá trình lấy thông tin tất cả đơn đặt hàng", e);
            }
        }

        public async Task<IEnumerable<OrderWithTotalCount>> GetOrdersByUserEmailAsync(OrderParams orderParams)
        {
            string userEmail = _userService.GetUserEmailFromClaims();
            
            var parameters = new DynamicParameters();
            parameters.Add("@UserEmail", userEmail, dbType: DbType.String, size: 255);
            parameters.Add("@PageIndex", orderParams.PageIndex, dbType: DbType.Int32);
            parameters.Add("@PageSize", orderParams.PageSize, dbType: DbType.Int32);
            parameters.Add("@Sort", orderParams.Sort, dbType: DbType.String, size: 50);
            parameters.Add("@OrderStatus", orderParams.OrderStatus, dbType: DbType.String, size: 100);

            string spName = "sp_GetOrdersByUserEmail";

            try
            {
                return await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<OrderWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy thông tin đơn đặt của người dùng '{email}': {Exception}", userEmail, e);
               
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy thông tin đơn đặt của người dùng theo email", e);
            }
        }

        public async Task<StoreProcedureResult> AcceptOrderAsync(int orderId, string userEmail)
        {
            var orderStatus = OrderStatus.PaymentReceived.ToString();

            var parameters = new DynamicParameters();
            parameters.Add("@UserEmail", userEmail, dbType: DbType.String, size: 255);
            parameters.Add("@OrderId", orderId, dbType: DbType.Int32);
            parameters.Add("@OrderStatus", orderStatus, dbType: DbType.String, size: 100);

            string spName = "sp_AcceptOrderByOrderIdAndUserEmail";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật trạng thái đơn hàng '#{orderId}' của người dùng '{email}': {Exception}",orderId, userEmail, e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật trạng thái đơn hàng của người dùng theo email", e);
            }
        }

        public async Task<StoreProcedureResult> DeclineOrderAsync(int orderId, string userEmail)
        {
            var orderStatus = OrderStatus.PaymentFailed.ToString();

            var parameters = new DynamicParameters();
            parameters.Add("@UserEmail", userEmail, dbType: DbType.String, size: 255);
            parameters.Add("@OrderId", orderId, dbType: DbType.Int32);
            parameters.Add("@OrderStatus", orderStatus, dbType: DbType.String, size: 100);

            string spName = "sp_DeclineOrderByOrderIdAndUserEmail";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật trạng thái đơn hàng '#{orderId}' của người dùng '{email}': {Exception}", orderId, userEmail, e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật trạng thái đơn hàng của người dùng theo email", e);
            }
        }
    }
}
