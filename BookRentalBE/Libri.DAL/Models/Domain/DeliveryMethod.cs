namespace Libri.DAL.Models.Domain
{
    [Serializable]
    public partial class DeliveryMethod
    {
        public DeliveryMethod()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; } = null!;
        public string DeliveryTime { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}