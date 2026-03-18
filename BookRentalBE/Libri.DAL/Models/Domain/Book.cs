namespace Libri.DAL.Models.Domain
{
    public partial class Book
    {
        public Book()
        {
            ExportHistories = new HashSet<ExportHistory>();
            ImportHistories = new HashSet<ImportHistory>();
            Inventories = new HashSet<Inventory>();
            BookImages = new HashSet<BookImage>();
            InventoryAuditDetails = new HashSet<InventoryAuditDetail>();
            InventoryAuditResults = new HashSet<InventoryAuditResult>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int? PublishYear { get; set; }
        public decimal Price { get; set; }
        public bool? IsAvailable { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? GenreId { get; set; }
        public string? Isbn { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual Genre? Genre { get; set; }
        public virtual Publisher Publisher { get; set; } = null!;
        public virtual ICollection<ExportHistory> ExportHistories { get; set; }
        public virtual ICollection<ImportHistory> ImportHistories { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryAuditDetail> InventoryAuditDetails { get; set; }
        public virtual ICollection<InventoryAuditResult> InventoryAuditResults { get; set; }
        public virtual ICollection<BookImage> BookImages { get; set; }
    }
}
