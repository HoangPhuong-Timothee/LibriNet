using Libri.DAL.Enums;
using Libri.DAL.Models.Custom.CustomOrder;

namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class Order
    {
        public string UserEmail { get; set; }
        public List<OrderItem> OrderItems { get; set; } 
        public int DeliveryMethodId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }      
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
    }
    
    [Serializable]
    public class OrderItem
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}