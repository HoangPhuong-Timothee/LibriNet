namespace Libri.DAL.Models.Custom.CustomInventory
{
    public class BookInventoryWithTotalCount
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string Isbn { get; set; }
        public string StoreName { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int TotalCount { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
