namespace Libri.DAL.Models.Domain
{
    public  partial class InventoryAuditResult
    {
        public int Id { get; set; }
        public int InventoryAuditDetailsId { get; set; }
        public int BookId { get; set; }
        public int BookStoreId { get; set; }
        public int? InventoryQuantity { get; set; }
        public int? ActualQuantity { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int? Difference { get; set; }
        public string? ResultDetails { get; set; }
        public DateTime? ConductedAt { get; set; }
        public string? ConductedBy { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual BookStore BookStore { get; set; } = null!;
        public virtual InventoryAuditDetail InventoryAuditDetails { get; set; } = null!;
        public virtual UnitOfMeasure UnitOfMeasure { get; set; } = null!;
    }
}
