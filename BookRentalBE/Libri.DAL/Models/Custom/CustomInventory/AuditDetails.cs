namespace Libri.DAL.Models.Custom.CustomInventory
{
    public class AuditDetails
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string Isbn { get; set; }
        public string StoreName { get; set; }
        public int InventoryQuantity { get; set; }
        public string MeasureUnit { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
