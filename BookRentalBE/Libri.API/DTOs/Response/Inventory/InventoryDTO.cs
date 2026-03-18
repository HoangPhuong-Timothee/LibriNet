namespace Libri.API.DTOs.Response.Inventory
{
    public class InventoryDTO
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string Isbn { get; set; }
        public string StoreName { get; set; }
        public int Quantity { get; set; }
        public string InventoryStatus { get; set; }
        public string UnitOfMeasure { get; set; }
        public string CreateInfo { get; set; }
        public string UpdateInfo { get; set; }
    }
}
