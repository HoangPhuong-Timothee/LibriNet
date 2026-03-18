namespace Libri.DAL.Models.Domain
{
    public partial class Inventory
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public int? BookStoreId { get; set; }
        public int? UnitOfMeasureId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual BookStore? BookStore { get; set; }
        public virtual UnitOfMeasure? UnitOfMeasure { get; set; }
    }
}