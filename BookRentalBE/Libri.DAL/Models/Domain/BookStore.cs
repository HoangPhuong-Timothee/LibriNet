namespace Libri.DAL.Models.Domain
{
    public partial class BookStore
    {
        public BookStore()
        {
            ExportHistories = new HashSet<ExportHistory>();
            ImportHistories = new HashSet<ImportHistory>();
            Inventories = new HashSet<Inventory>();
            InventoryAuditDetails = new HashSet<InventoryAuditDetail>();
            InventoryAuditResults = new HashSet<InventoryAuditResult>();
        }

        public int Id { get; set; }
        public string StoreName { get; set; } = null!;
        public string StoreAddress { get; set; } = null!;
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = null!;

        public virtual ICollection<ExportHistory> ExportHistories { get; set; }
        public virtual ICollection<ImportHistory> ImportHistories { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryAuditDetail> InventoryAuditDetails { get; set; }
        public virtual ICollection<InventoryAuditResult> InventoryAuditResults { get; set; }
    }
}