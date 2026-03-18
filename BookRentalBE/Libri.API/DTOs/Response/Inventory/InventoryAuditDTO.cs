namespace Libri.API.DTOs.Response.Inventory
{
    public class InventoryAuditDTO
    {
        public int Id { get; set; }
        public string AuditDate { get; set; }
        public string AudittedBy { get; set; }
        public string AuditStatus { get; set; }
        public string CreatedAt { get; set; }
        public string AuditNotes { get; set; }
        public string AuditCode { get; set; }
    }
}
