namespace Libri.API.DTOs.Response.Inventory
{
    public class InventoryAuditDetailsDTO
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string Isbn { get; set; }
        public string StoreName { get; set; }
        public int InventoryQuantity { get; set; }
        public string MeasureUnit { get; set; }
        public string UpdateInfo { get; set; }
    }
}
