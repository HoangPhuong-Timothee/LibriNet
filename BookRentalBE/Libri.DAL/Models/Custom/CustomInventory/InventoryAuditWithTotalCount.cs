namespace Libri.DAL.Models.Custom.CustomInventory
{
    public class InventoryAuditWithTotalCount
    {
        public int Id { get; set; }
        public DateTime AuditDate { get; set; }
        public string AudittedBy { get; set; }
        public string AuditStatus { get; set; }
        public string AuditNotes { get; set; }
        public string AuditCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalCount { get; set; }
    }
}
