namespace Libri.DAL.Models.Domain
{
    public partial class ExportHistory
    {
        public int Id { get; set; }
        public int BookStoreId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string PerformedBy { get; set; } = null!;
        public DateTime ExportDate { get; set; }
        public string ExportNotes { get; set; } = null!;
        public int UnitOfMeasureId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual BookStore BookStore { get; set; } = null!;
        public virtual UnitOfMeasure UnitOfMeasure { get; set; } = null!;
    }
}
