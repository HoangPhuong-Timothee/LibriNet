namespace Libri.API.DTOs.Response.Order
{
    public class OrderWithDetailsDTO
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public string OrderDate { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public decimal Subtotal { get; set; }
        public string DeliveryShortName { get; set; }
        public string DeliveryTime { get; set; }
        public decimal DeliveryPrice { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
