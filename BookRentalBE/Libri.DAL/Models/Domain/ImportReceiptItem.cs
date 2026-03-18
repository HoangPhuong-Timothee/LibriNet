namespace Libri.DAL.Models.Domain
{
    public partial class ImportReceiptItem
    {
        public int Id { get; set; }
        public int ImportReceiptId { get; set; }
        public int BookId { get; set; }
        public int BookStoreId { get; set; }
        public int Quantity { get; set; }
        public int UnitOfMeasureId { get; set; }

        public virtual ImportInventoryReceipt ImportReceipt { get; set; } = null!;
    }
}
