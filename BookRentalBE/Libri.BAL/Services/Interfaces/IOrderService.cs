using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomOrder;
using Libri.DAL.Models.Domain;

namespace Libri.BAL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<StoreProcedureResult> CreateOrderAsync(string basketId, Order order);
        Task<IEnumerable<OrderWithTotalCount>> GetAllOrdersForAdminAsync(OrderParams orderParams);
        Task<IEnumerable<OrderWithTotalCount>> GetOrdersByUserEmailAsync(OrderParams orderParams);
        Task<OrderWithDetails> GetOrderByOrderIdAndUserEmailAsync(int orderId);
        Task<StoreProcedureResult> AcceptOrderAsync(int orderId, string userEmail);
        Task<StoreProcedureResult> DeclineOrderAsync(int orderId, string userEmail);
    }
}
