namespace Libri.DAL.Models.Domain
{
    public partial class InventoryAuditDetail
    {
        public InventoryAuditDetail()
        {
            InventoryAuditResults = new HashSet<InventoryAuditResult>();
        }

        public int Id { get; set; }
        public int InventoryAuditId { get; set; }
        public int BookId { get; set; }
        public string Isbn { get; set; }
        public int BookStoreId { get; set; }
        public int? InventoryQuantity { get; set; }
        public int UnitOfMeasureId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual BookStore BookStore { get; set; } = null!;
        public virtual InventoryAudit InventoryAudit { get; set; } = null!;
        public virtual UnitOfMeasure UnitOfMeasure { get; set; } = null!;
        public virtual ICollection<InventoryAuditResult> InventoryAuditResults { get; set; }
    }
}
