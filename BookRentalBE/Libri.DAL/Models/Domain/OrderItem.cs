namespace Libri.DAL.Models.Domain
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int? BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? OrderId { get; set; }

        public virtual Order? Order { get; set; }
    }
}
