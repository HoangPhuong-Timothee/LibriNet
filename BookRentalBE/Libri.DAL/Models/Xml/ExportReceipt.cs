using Libri.DAL.Enums;

namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class ExportReceipt
    {
        public string ExportNotes { get; set; }
        public InventoryReceiptStatus ReceiptStatus { get; set; } = InventoryReceiptStatus.Pending;
        public List<ExportReceiptItem> ExportReceiptItems { get; set; } = new List<ExportReceiptItem>();
    }

    public class ExportReceiptItem 
    {
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int BookStoreId { get; set; }
        public string ISBN { get; set; }
    }
}