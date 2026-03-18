namespace Libri.DAL.Models.Domain
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string? UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string? FullName { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public int DeliveryMethodId { get; set; }
        public decimal Subtotal { get; set; }
        public string Status { get; set; } = null!;
        public string? PaymentIntentId { get; set; }

        public virtual DeliveryMethod? DeliveryMethod { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
