namespace Libri.DAL.Models.Domain
{
    public partial class UnitOfMeasure
    {
        public UnitOfMeasure()
        {
            ExportHistories = new HashSet<ExportHistory>();
            ImportHistories = new HashSet<ImportHistory>();
            Inventories = new HashSet<Inventory>();
            InventoryAuditDetails = new HashSet<InventoryAuditDetail>();
            InventoryAuditResults = new HashSet<InventoryAuditResult>();
            UnitMappingDestUnits = new HashSet<UnitMapping>();
            UnitMappingSrcUnits = new HashSet<UnitMapping>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual ICollection<ExportHistory> ExportHistories { get; set; }
        public virtual ICollection<ImportHistory> ImportHistories { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryAuditDetail> InventoryAuditDetails { get; set; }
        public virtual ICollection<InventoryAuditResult> InventoryAuditResults { get; set; }
        public virtual ICollection<UnitMapping> UnitMappingDestUnits { get; set; }
        public virtual ICollection<UnitMapping> UnitMappingSrcUnits { get; set; }
    }
}

