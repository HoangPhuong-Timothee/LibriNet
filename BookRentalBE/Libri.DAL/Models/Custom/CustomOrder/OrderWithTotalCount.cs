namespace Libri.DAL.Models.Custom.CustomOrder
{
    public class OrderWithTotalCount
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public int Subtotal { get; set; }
        public string Status { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string PaymentIntentId { get; set; }
        public string DeliveryShortName { get; set; }
        public int TotalCount { get; set; }
    }
}