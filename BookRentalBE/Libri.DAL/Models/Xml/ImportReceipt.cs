using Libri.DAL.Enums;

namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class ImportReceipt
    {
        public string ImportNotes { get; set; }
        public InventoryReceiptStatus ReceiptStatus { get; set; } = InventoryReceiptStatus.Pending;
        public List<ImportReceiptItem> ImportReceiptItems { get; set; } = new List<ImportReceiptItem>();
    }

    [Serializable]
    public class ImportReceiptItem 
    {
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int BookStoreId { get; set; }
        public string ISBN { get; set; }
    }
}