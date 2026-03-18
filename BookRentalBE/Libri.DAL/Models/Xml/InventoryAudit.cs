namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class InventoryAudit
    {
        public string AudittedBy { get; set; }
        public DateTime AuditDate { get; set; }
        public string AuditNotes { get; set; }
        public List<InventoryAuditItem> InventoryAuditItems { get; set; } = new List<InventoryAuditItem>();
    }

    [Serializable]
    public class InventoryAuditItem
    {
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public int BookStoreId { get; set; }
    }
}
