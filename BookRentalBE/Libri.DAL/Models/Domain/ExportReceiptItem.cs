
namespace Libri.DAL.Models.Domain
{
    public partial class ExportReceiptItem
    {
        public int Id { get; set; }
        public int ExportReceiptId { get; set; }
        public int BookId { get; set; }
        public int BookStoreId { get; set; }
        public int Quantity { get; set; }
        public int UnitOfMeasureId { get; set; }

        public virtual ExportInventoryReceipt ExportReceipt { get; set; } = null!;
    }
}
