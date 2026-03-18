namespace Libri.DAL.Models.Domain
{
    public partial class InventoryAudit
    {
        public InventoryAudit()
        {
            InventoryAuditDetails = new HashSet<InventoryAuditDetail>();
        }

        public int Id { get; set; }
        public string? AuditStatus { get; set; }
        public DateTime AuditDate { get; set; }
        public string? AudittedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? AuditNotes { get; set; }
        public string? AuditCode { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<InventoryAuditDetail> InventoryAuditDetails { get; set; }
    }
}
